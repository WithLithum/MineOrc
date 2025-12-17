// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest;
using MineOrc.Resources;

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
        catch (HttpRequestException ex)
        {
            MyOutput.Error(ex.Message, Texts.InstallManifestFailCannotGetJson);
            return false;
        }

        var excerpt = manifest.Versions.FirstOrDefault(x => x.Id == _installName);
        if (excerpt == null)
        {
            MyOutput.Error(Texts.FormatInstallManifestFailNoSuchVersion(_installName));
            return false;
        }

        try
        {
            await SaveManifestAsync(excerpt, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            MyOutput.Error(Texts.InstallManifestFailCannotGetJson,
                ex.StatusCode?.ToString("D")
                ?? ex.Message);
            return false;
        }
        catch (IOException ioe)
        {
            MyOutput.Error(ioe, Texts.InstallManifestFailCannotSaveJson);
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
            cancellationToken).ConfigureAwait(false);
        var targetStream = GameApplication.Versions.CreateManifest(excerpt.Id);

        await using (originStream.ConfigureAwait(false))
        await using (targetStream.ConfigureAwait(false))
        {
            await originStream.CopyToAsync(targetStream, cancellationToken).ConfigureAwait(false);
        }
    }
}