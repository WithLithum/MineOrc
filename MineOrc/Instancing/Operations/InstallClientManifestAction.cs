// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest;
using MineOrc.Resources;
using SmartFormat;

namespace MineOrc.Instancing.Operations;

public sealed class InstallClientManifestAction : IAsyncForegroundAction
{
    private readonly string _installName;

    public InstallClientManifestAction(string installName)
    {
        _installName = installName;
    }

    public string Name => "installClientManifest";

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
            MyOutput.Error(ex, Texts.InstallManifestFailCannotGetJson);
            return false;
        }

        var excerpt = manifest.Versions.FirstOrDefault(x => x.Id == _installName);
        if (excerpt == null)
        {
            MyOutput.Error(Smart.Format(Texts.InstallManifestFailNoSuchVersion,
                new { Version = _installName }));
            return false;
        }

        try
        {
            await SaveManifestAsync(excerpt, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            MyOutput.Error(ex, Texts.InstallManifestFailCannotGetJson);
            return false;
        }

        return true;
    }

    private static async Task SaveManifestAsync(VersionExcerpt excerpt,
        CancellationToken cancellationToken)
    {
        if (await GameApplication.Versions.ValidateManifestAsync(excerpt)
                .ConfigureAwait(false))
        {
            return;
        }
        
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