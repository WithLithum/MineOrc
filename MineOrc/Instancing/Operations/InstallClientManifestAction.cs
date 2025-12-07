// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest;

namespace MineOrc.Instancing.Operations;

public sealed class InstallClientManifestAction : IAsyncForegroundAction
{
    private readonly string _installName;

    public InstallClientManifestAction(string installName)
    {
        _installName = installName;
    }

    public string Name => "install client manifest";

    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        VersionManifest manifest;
        try
        {
            manifest = await MineOrcApp.PistonMetaClient.GetVersionManifestAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            MyOutput.Error(ex, "cannot get manifest");
            return false;
        }

        var excerpt = manifest.Versions.FirstOrDefault(x => x.Id == _installName);
        if (excerpt == null)
        {
            MyOutput.Error("version '{0}' not found", _installName);
            return false;
        }

        try
        {
            await SaveManifestAsync(excerpt, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            MyOutput.Error(ex, "cannot download manifest");
            return false;
        }

        return true;
    }

    private static async Task SaveManifestAsync(VersionExcerpt excerpt,
        CancellationToken cancellationToken)
    {
        var originStream = await MineOrcApp.HttpClient.GetStreamAsync(excerpt.Url,
            cancellationToken);
        var targetStream = GameApplication.Versions.CreateManifest(excerpt.Id);
        
        await using (originStream.ConfigureAwait(false))
        await using (targetStream.ConfigureAwait(false))
        {
            await originStream.CopyToAsync(targetStream, cancellationToken).ConfigureAwait(false);
        }
    }
}