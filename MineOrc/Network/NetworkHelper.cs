// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Downloader;
using JetBrains.Annotations;
using Meziantou.Framework;
using MineOrc.Foundation.Network.Results;
using Spectre.Console;

namespace MineOrc.Network;

public static class NetworkHelper
{
    [MustUseReturnValue("Network request may fail")]
    public static async Task<NetworkResult> DownloadFileForegroundAsync(Uri fromUri,
        string toFile,
        CancellationToken cancellationToken = default)
    {
        var progress = AnsiConsole.Progress();
        var downloader = new DownloadBuilder()
            .WithUrl(fromUri)
            .WithFileLocation(toFile)
            .Build();

        await using (downloader.ConfigureAwait(false))
        {
            try
            {
                await progress.StartAsync(async context =>
                        await DownloadFileForegroundInternalAsync(context, downloader, cancellationToken)
                            .ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
            catch (Exception e) when (e is HttpRequestException
                                          or HttpIOException
                                          or IOException)
            {
                return NetworkResult.FromException(e);
            }
        }

        return NetworkResult.Ok;
    }

    private static async Task DownloadFileForegroundInternalAsync(ProgressContext c,
        IDownload downloader,
        CancellationToken cancellationToken)
    {
        var task = c.AddTask("N/A");

        downloader.DownloadProgressChanged += (_, args) =>
        {
            task.Increment(args.ProgressPercentage - task.Percentage);
            var byteSpeed = new ByteSize((int)args.AverageBytesPerSecondSpeed);

            task.Description = $"{byteSpeed}/s";
        };
        downloader.DownloadFileCompleted += (_, _) =>
            task.StopTask();

        task.StartTask();
        await downloader.StartAsync(cancellationToken).ConfigureAwait(false);
    }
}