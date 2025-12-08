// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using JetBrains.Annotations;
using MineOrc.Foundation.Manifest.Resources;
using MineOrc.Foundation.Network.Results;
using MineOrc.Foundation.Runtime.Resources;
using MineOrc.Foundation.Utilities;
using MineOrc.Network;
using Spectre.Console;

namespace MineOrc.Instancing.Assets;

internal sealed class AssetsRestorer : QueueDispatchAction<KeyValuePair<string, AssetInfo>>
{
    private static readonly Uri ResourceDownloadBase = new("https://resources.download.minecraft.net/");

    private readonly IProgressEx<double> _progress;

    public AssetsRestorer(AssetIndexDictionary index,
        IProgressEx<double> progress) : base(index)
    {
        _progress = progress;
    }

    #region Verify & download

    private async ValueTask<bool> VerifyAsync(AssetInfo asset,
        CancellationToken cancellationToken = default)
    {
        if (!GameApplication.Assets.HasAssetObject(asset))
        {
            return false;
        }

        try
        {
            return await HashHelper.VerifyFileAsync(GameApplication.Assets.GetAssetObjectFile(asset),
                    asset.Hash,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        catch (IOException ex)
        {
            MyOutput.Error(ex, "error while verifying asset");
            return false;
        }
    }

    [MustUseReturnValue]
    private async Task<bool> DownloadAsync(AssetInfo assetInfo,
        string key,
        CancellationToken cancellationToken)
    {
        var downloadUri = new Uri(ResourceDownloadBase, $"{assetInfo.Hash[..2]}/{assetInfo.Hash}");
        var targetPath = GameApplication.Assets.GetAssetObjectFile(assetInfo);
        GameApplication.Assets.CreatePrefix(assetInfo);

        var result = await NetworkHelper.DownloadFileSilentAsync(downloadUri,
            targetPath,
            cancellationToken).ConfigureAwait(false);
        if (!result.IsOk)
        {
            MyOutput.Error(result.ToString());
            return false;
        }
        
        AnsiConsole.WriteLine($"Downloaded {key}");
        
        return true;
    }

    #endregion

    protected override async Task<bool> ExecuteActionAsync(KeyValuePair<string, AssetInfo> payload,
        CancellationToken cancellationToken)
    {
        var (key, assetInfo) = payload;

        // Verify
        if (await VerifyAsync(assetInfo, cancellationToken).ConfigureAwait(false))
        {
            return true;
        }

        // Download
        return await DownloadAsync(assetInfo, key, cancellationToken).ConfigureAwait(false);
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