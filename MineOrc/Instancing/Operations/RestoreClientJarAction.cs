// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Downloader;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Manifest.Network;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Instancing.Operations;

public class RestoreClientJarAction : IAsyncForegroundAction
{
    private readonly string _versionId;

    public RestoreClientJarAction(string versionId)
    {
        _versionId = versionId;
    }

    public string Name => "restoreClientJar";
    
    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        if (!GameApplication.Versions.Exists(_versionId))
        {
            MyOutput.Error(Texts.RestoreClientJarFailNoManifest);
            return false;
        }

        var manifest = await GameApplication.Versions.GetManifestAsync(_versionId,
            cancellationToken)
            .ConfigureAwait(false);

        var clientArtefact = manifest.GetClientDownload();
        if (clientArtefact == null)
        {
            MyOutput.Error(Texts.RestoreClientJarFailNoArtefact);
            return false;
        }

        if (await GameApplication.Versions.ValidateJarAsync(manifest.Id, clientArtefact)
                .ConfigureAwait(false))
        {
            // Valid client, no need to download
            return true;
        }
        
        // Download
        try
        {
            await DoDownloadAsync(clientArtefact, manifest, cancellationToken);
        }
        catch (Exception ex)
        {
            MyOutput.Error(ex, Texts.OperationDownloadFail);
            return false;
        }

        return true;
    }

    private static async Task DoDownloadAsync(ArtefactInfo clientArtefact,
        ClientManifest manifest,
        CancellationToken cancellationToken)
    {
        var progress = AnsiConsole.Progress();
        var downloader = new DownloadBuilder()
            .WithUrl(clientArtefact.Url)
            .WithFileLocation(GameApplication.Versions.GetJarPath(manifest.Id))
            .Build();

        await progress.StartAsync(async c =>
            {
                var task = c.AddTask(Texts.RestoreClientJarAction);
                
                downloader.DownloadProgressChanged += (_, args) =>
                    task.Increment(args.ProgressPercentage - task.Percentage);
                downloader.DownloadFileCompleted += (_, _) =>
                    task.StopTask();
                
                task.StartTask();
                await downloader.StartAsync(cancellationToken).ConfigureAwait(false);
            })
            .ConfigureAwait(false);
    }
}