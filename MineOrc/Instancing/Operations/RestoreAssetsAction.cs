// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using MineOrc.Foundation.Manifest.Network;
using MineOrc.Foundation.Manifest.Resources;
using MineOrc.Foundation.Network.Results;
using MineOrc.Foundation.Utilities;
using MineOrc.Instancing.Assets;
using MineOrc.Network;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Instancing.Operations;

public sealed class RestoreAssetsAction : QueueDispatchActionWrapper<KeyValuePair<string, AssetInfo>>
{
    private readonly AssetIndexArtefactInfo _indexArtefact;

    public RestoreAssetsAction(AssetIndexArtefactInfo artefact) : base("restoreAssets")
    {
        _indexArtefact = artefact;
    }

    protected override async ValueTask<IReadOnlyCollection<KeyValuePair<string, AssetInfo>>?>
        GetPayloadsAsync(CancellationToken cancellationToken)
    {
        if (!GameApplication.Assets.HasAssetIndex(_indexArtefact.Id)
            && !await RestoreIndexAsync(cancellationToken).ConfigureAwait(false))
        {
            return null;
        }
        
        AssetManifest index;
        try
        {
            var readIndex = await GameApplication.Assets.GetAssetIndexAsync(_indexArtefact.Id,
                cancellationToken).ConfigureAwait(false);
            
            if (readIndex == null)
            {
                MyOutput.Error(Texts.FormatAssetRestoreMissingIndex(_indexArtefact.Id));
                return null;
            }

            index = readIndex;
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            MyOutput.Error(ex, Texts.FormatAssetRestoreIndexReadFailed(_indexArtefact.Id));
            return null;
        }

        return index.Objects;
    }

    protected override QueueDispatchAction<KeyValuePair<string, AssetInfo>> CreateAction(IReadOnlyCollection<KeyValuePair<string, AssetInfo>> payload)
    {
        return new AssetsRestorer(payload);
    }

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
}