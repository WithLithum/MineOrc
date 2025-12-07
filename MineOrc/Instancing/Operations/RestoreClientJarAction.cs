// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Manifest.Network;
using MineOrc.Network;
using MineOrc.Resources;

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
        return await DoDownloadAsync(clientArtefact, manifest, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<bool> DoDownloadAsync(ArtefactInfo clientArtefact,
        ClientManifest manifest,
        CancellationToken cancellationToken)
    {
        try
        {
            await NetworkHelper.DownloadFileForegroundAsync(clientArtefact.Url,
                    GameApplication.Versions.GetJarPath(manifest.Id),
                    cancellationToken)
                .ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            MyOutput.Error(Texts.OperationDownloadFailHttp, ex.StatusCode?.ToString("D")
                                                            ?? ex.HttpRequestError.ToString("G"));
            return false;
        }
        catch (IOException ex)
        {
            MyOutput.Error(ex, Texts.OperationDownloadFail);
            return false;
        }

        return true;
    }
}