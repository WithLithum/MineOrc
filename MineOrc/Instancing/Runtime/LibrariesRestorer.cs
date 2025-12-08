// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Network.Results;
using MineOrc.Foundation.Platforms;
using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Utilities;
using MineOrc.Network;
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
        try
        {
            if (await _libraryManager.VerifyArtefactAsync(artefactInfo,
                    cancellationToken).ConfigureAwait(false))
            {
                return true;
            }
        }
        catch (IOException io)
        {
            MyOutput.Warn(io, Texts.OperationHashFail);
        }

        var target = _libraryManager.GetArtefactPath(artefactInfo);
        var result = await NetworkHelper.DownloadFileForegroundAsync(artefactInfo.Url,
            target,
            cancellationToken
        ).ConfigureAwait(false);
        
        if (!result.IsOk)
        {
            MyOutput.Error(result.ToString());
            return false;
        }

        return true;
    }
}