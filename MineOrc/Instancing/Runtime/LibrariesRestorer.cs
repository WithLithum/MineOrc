// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Platforms;
using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Utilities;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Instancing.Runtime;

internal sealed class LibrariesRestorer : QueueDispatchAction<LibraryInfo>
{
    private readonly LibraryManager _libraryManager;

    public LibrariesRestorer(IReadOnlyCollection<LibraryInfo> payloads,
        LibraryManager libraryManager) : base(payloads)
    {
        _libraryManager = libraryManager;
    }

    protected override async Task<bool> ExecuteActionAsync(LibraryInfo payload, CancellationToken cancellationToken)
    {
        var primary = payload.Downloads.Artifact;
        LibraryArtefactInfo? native = null;

        if (payload is { Natives: not null, Downloads.Classifiers: not null })
        {
            var nativeKey = payload.Natives[ManifestPlatformUtil.GetSystemName()];
            native = payload.Downloads.Classifiers[nativeKey];
        }

        var success = await DoFileAsync(primary, cancellationToken).ConfigureAwait(false);
        if (native != null)
        {
            success = await DoFileAsync(native, cancellationToken).ConfigureAwait(false);
        }

        if (success)
        {
            AnsiConsole.WriteLine(Texts.RestoreLibraryDownloaded, payload.Name);
        }

        return success;
    }

    private async Task<bool> DoFileAsync(LibraryArtefactInfo artefactInfo,
        CancellationToken cancellationToken)
    {
        if (await _libraryManager.VerifyArtefactAsync(artefactInfo,
                cancellationToken).ConfigureAwait(false))
        {
            return true;
        }

        try
        {
            await DownloadAsync(artefactInfo, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            MyOutput.DownloadError(ex);
            return false;
        }
        catch (IOException ex)
        {
            MyOutput.Error(ex, Texts.OperationDownloadFail);
            return false;
        }

        return true;
    }

    private async Task DownloadAsync(LibraryArtefactInfo artefactInfo, CancellationToken cancellationToken)
    {
        var source = await MineOrcApp.HttpClient.GetStreamAsync(artefactInfo.Url,
            cancellationToken).ConfigureAwait(false);
        var target = _libraryManager.CreateArtefact(artefactInfo);

        await using (source.ConfigureAwait(false))
        await using (target.ConfigureAwait(false))
        {
            await source.CopyToAsync(target, cancellationToken).ConfigureAwait(false);
        }
    }
}