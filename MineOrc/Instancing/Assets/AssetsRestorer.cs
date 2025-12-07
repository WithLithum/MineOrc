// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Buffers;
using System.Security.Cryptography;
using MineOrc.Foundation.Manifest.Resources;
using MineOrc.Foundation.Runtime.Resources;
using MineOrc.Foundation.Utilities;
using Spectre.Console;

namespace MineOrc.Instancing.Assets;

public class AssetsRestorer
{
    private static readonly Uri ResourceDownloadBase = new("https://resources.download.minecraft.net/");
    
    private const int SmallBufferSize = 32;

    private const int MediumBufferSize = 256;
    private const int MediumBufferThreshold = 1024;

    private const int LargeBufferSize = 1024;
    private const int LargeBufferThreshold = LargeBufferSize * 10;

    private readonly IProgressEx<double> _progress;
    private readonly AssetIndexDictionary _index;
    private readonly AssetManager _assetManager;
    private readonly HttpClient _httpClient;

    public AssetsRestorer(AssetManager assetManager,
        AssetIndexDictionary index,
        IProgressEx<double> progress, HttpClient httpClient)
    {
        _assetManager = assetManager;
        _index = index;
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

    public async Task<bool> DoRestoreAsync(CancellationToken cancellationToken = default)
    {
        var semaphore = new SemaphoreSlim(3, 3);
        var queue = await AllocateAsync(cancellationToken).ConfigureAwait(false);
        var counter = 0;
        var success = true;
        
        await AllocateAsync(cancellationToken);

        while (true)
        {
            if (!queue.TryDequeue(out var assetPair))
            {
                break;
            }
            
            await semaphore.WaitAsync(cancellationToken)
                .ConfigureAwait(false);

            // We want to download everything and fail at the end.
            try
            {
                await ExecuteDownloadAsync(assetPair.Item2, cancellationToken)
                    .ConfigureAwait(false);
                counter++;
                AnsiConsole.WriteLine($"Downloaded {assetPair.Item1}");
            }
            catch (HttpRequestException ex)
            {
                MyOutput.Error(ex.StatusCode != null
                    ? $"http error {ex.StatusCode:D} when downloading asset"
                    : $"error when downloading asset: {ex.Message}");
                success = false;
            }
            catch (Exception ex)
            {
                MyOutput.Error(ex, "error when downloading asset");
                success = false;
            }

            semaphore.Release();
            
            // Report current progress.
            var currentProgress = (double)counter / _index.Count * 100;
            _progress.Report(currentProgress);
        }

        AnsiConsole.WriteLine("{0}/{1} downloaded", counter, _index.Count);
        return success;
    }
    
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
            await using var stream = File.OpenRead(file);
            var hash = Convert.ToHexStringLower(await SHA1.HashDataAsync(stream, cancellationToken)
                .ConfigureAwait(false));

            return hash.Equals(assetObj, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception)
        {
            return false;
        }
    }

    private async Task ExecuteDownloadAsync(AssetInfo assetInfo,
        CancellationToken cancellationToken = default)
    {
        // Verify
        if (await VerifyAsync(assetInfo.Hash, cancellationToken).ConfigureAwait(false))
        {
            return;
        }

        // Download
        var downloadUri = new Uri(ResourceDownloadBase, $"{assetInfo.Hash[..2]}/{assetInfo.Hash}");
        
        using var bufferLease = MemoryPool<byte>.Shared
            .Rent(GetBufferSize(assetInfo.Size));
        await using var remote = await _httpClient.GetStreamAsync(downloadUri, cancellationToken);
        await using var target = _assetManager.CreateAssetObject(assetInfo.Hash);
        while (true)
        {
            var read = await remote.ReadAsync(bufferLease.Memory, cancellationToken);
            if (read == 0)
            {
                break;
            }
            
            await target.WriteAsync(bufferLease.Memory[..read], cancellationToken);
        }
    }

    private async Task<Queue<(string, AssetInfo)>> AllocateAsync(CancellationToken cancellationToken = default)
    {
        var queue = new Queue<(string, AssetInfo)>();
        
        await Parallel.ForEachAsync(_index,
            cancellationToken,
            (pair, _) =>
            {
                queue.Enqueue((pair.Key, pair.Value));
                return ValueTask.CompletedTask;
            });

        return queue;
    }
}