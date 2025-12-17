// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Manifest.Network;
using MineOrc.Foundation.Network.Results;
using MineOrc.Foundation.Runtime;
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

        var verify = await GameApplication.Versions.ValidateJarAsync(manifest.Id, clientArtefact)
            .ConfigureAwait(false);
        switch (verify)
        {
            case VerifyResult.NoHash:
                MyOutput.Notice(Texts.FormatOperationNoticeNoHash(_versionId));
                return true;
            case VerifyResult.Intact:
                return true;
            case VerifyResult.Damaged:
                // Download
                return await DoDownloadAsync(clientArtefact, manifest, cancellationToken).ConfigureAwait(false);
            default:
                throw new InvalidOperationException($"Unknown verify result '{verify:D}'");
        }
    }

    private static async Task<bool> DoDownloadAsync(ArtefactInfo clientArtefact,
        ClientManifest manifest,
        CancellationToken cancellationToken)
    {
        var result = await NetworkHelper.DownloadFileForegroundAsync(clientArtefact.Url,
                GameApplication.Versions.GetJarPath(manifest.Id),
                cancellationToken)
            .ConfigureAwait(false);

        if (!result.IsOk)
        {
            MyOutput.Error(result.ToString());
            return false;
        }

        return true;
    }
}