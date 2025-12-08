// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using MineOrc.Foundation.Manifest.Network;
using MineOrc.Foundation.Manifest.Resources;
using MineOrc.Foundation.Network.Results;
using MineOrc.Instancing.Assets;
using MineOrc.Network;
using MineOrc.Resources;
using MineOrc.UI;
using Spectre.Console;

namespace MineOrc.Instancing.Operations;

public sealed class RestoreAssetsAction : IAsyncForegroundAction
{
    private readonly AssetIndexArtefactInfo _indexArtefact;

    public RestoreAssetsAction(AssetIndexArtefactInfo artefact)
    {
        _indexArtefact = artefact;
    }

    public string Name => "restoreAssets";

    private async Task<bool> RestoreIndexAsync(CancellationToken cancellationToken)
    {
        var target = GameApplication.Assets.GetAssetIndexFile(_indexArtefact.Id);
        var source = _indexArtefact.Url;
        
        AnsiConsole.WriteLine(Texts.AssetRestoreDownloadIndex);
        var result = await NetworkHelper.DownloadFileSilentAsync(source, target,
            cancellationToken).ConfigureAwait(false);

        if (!result.IsOk)
        {
            MyOutput.Error(result.ToString());
        }

        return result.IsOk;
    }
    
    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        if (!GameApplication.Assets.HasAssetIndex(_indexArtefact.Id)
            && !await RestoreIndexAsync(cancellationToken).ConfigureAwait(false))
        {
            return false;
        }
        
        AssetManifest index;
        try
        {
            var readIndex = await GameApplication.Assets.GetAssetIndexAsync(_indexArtefact.Id,
                cancellationToken).ConfigureAwait(false);
            
            if (readIndex == null)
            {
                MyOutput.Error(Texts.AssetRestoreMissingIndex, _indexArtefact.Id);
                return false;
            }

            index = readIndex;
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            MyOutput.Error(ex, Texts.AssetRestoreIndexReadFailed);
            return false;
        }
        
        // Restore action
        var progress = AnsiConsole.Progress();
        var success = true;
        await progress.StartAsync(async c =>
        {
            var task = c.AddTask("???");
            var prog = new ProgressAction(task);

            var restorer = new AssetsRestorer(index.Objects,
                prog);
            success = await restorer.DoAsync(cancellationToken).ConfigureAwait(false);
        }).ConfigureAwait(false);

        return success;
    }
}