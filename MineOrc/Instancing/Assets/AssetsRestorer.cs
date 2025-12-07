// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Buffers;
using MineOrc.Foundation.Manifest.Resources;
using MineOrc.Foundation.Runtime.Resources;
using MineOrc.Foundation.Utilities;
using Spectre.Console;

using static MineOrc.Network.NetworkGlobals;

namespace MineOrc.Instancing.Assets;

internal sealed class AssetsRestorer : QueueDispatchAction<KeyValuePair<string, AssetInfo>>
{
    private static readonly Uri ResourceDownloadBase = new("https://resources.download.minecraft.net/");

    private readonly IProgressEx<double> _progress;
    private readonly AssetManager _assetManager;
    private readonly HttpClient _httpClient;

    public AssetsRestorer(AssetManager assetManager,
        AssetIndexDictionary index,
        IProgressEx<double> progress, HttpClient httpClient) : base(index)
    {
        _assetManager = assetManager;
        _progress = progress;
        _httpClient = httpClient;
    }

    private static int GetBufferSize(int size)
    {
        return size switch
        {
            >= LargeBufferThreshold => LargeBufferSize,
            >= MediumBufferThreshold => MediumBufferSize,
            _ => SmallBufferSize
        };
    }

    #region Verify & download
    
    private async ValueTask<bool> VerifyAsync(string assetObj,
        CancellationToken cancellationToken = default)
    {
        if (!_assetManager.HasAssetObject(assetObj))
        {
            return false;
        }

        var file = _assetManager.GetAssetObjectFile(assetObj);

        try
        {
            var stream = File.OpenRead(file);
            await using (stream.ConfigureAwait(false))
            {
                return await HashHelper.VerifyStreamAsync(stream, assetObj, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            MyOutput.Error(ex, "error while verifying asset");
            return false;
        }
    }

    private async Task DownloadAsync(AssetInfo assetInfo, CancellationToken cancellationToken)
    {
        var downloadUri = new Uri(ResourceDownloadBase, $"{assetInfo.Hash[..2]}/{assetInfo.Hash}");

        using var bufferLease = MemoryPool<byte>.Shared
            .Rent(GetBufferSize(assetInfo.Size));
        await using var remote = await _httpClient.GetStreamAsync(downloadUri, cancellationToken);
        await using var target = _assetManager.CreateAssetObject(assetInfo.Hash);

        await remote.CopyToAsync(target, cancellationToken).ConfigureAwait(false);
    }
    
    #endregion
    
    protected override async Task<bool> ExecuteActionAsync(KeyValuePair<string, AssetInfo> payload, CancellationToken cancellationToken)
    {
        var (key, assetInfo) = payload;
        
        // Verify
        if (await VerifyAsync(assetInfo.Hash, cancellationToken).ConfigureAwait(false))
        {
            return true;
        }

        // Download
        try
        {
            await DownloadAsync(assetInfo, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            MyOutput.Error(ex.StatusCode != null
                ? $"http error {ex.StatusCode:D} when downloading asset"
                : $"error when downloading asset: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            MyOutput.Error(ex, "error when downloading asset");
            return false;
        }
        
        AnsiConsole.WriteLine("Downloaded '{0}'", key);

        return true;
    }

    protected override void ReportProgress(double progress)
    {
        _progress.Report(progress);
    }

    protected override void ReportException(Exception exception)
    {
        MyOutput.Error(exception, "error while downloading");
    }
}