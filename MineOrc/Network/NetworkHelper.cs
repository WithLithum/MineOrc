// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Downloader;
using Meziantou.Framework;
using MineOrc.Instancing;
using Spectre.Console;

namespace MineOrc.Network;

public static class NetworkHelper
{
    public static async Task DownloadFileForegroundAsync(Uri fromUri,
        string toFile,
        CancellationToken cancellationToken = default)
    {
        var progress = AnsiConsole.Progress();
        var downloader = new DownloadBuilder()
            .WithUrl(fromUri)
            .WithFileLocation(toFile)
            .Build();

        await progress.StartAsync(async context =>
                await DownloadFileForegroundInternalAsync(context, downloader, cancellationToken)
                    .ConfigureAwait(false))
            .ConfigureAwait(false);
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