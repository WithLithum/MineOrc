// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Network.Results;
using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Utilities;
using MineOrc.Network;
using MineOrc.Resources;
using SmartFormat;
using Spectre.Console;

namespace MineOrc.Instancing.Runtime;

internal sealed class LibrariesRestorer : QueueDispatchAction<LibraryArtefactInfo>
{
    public LibrariesRestorer(IReadOnlyCollection<LibraryArtefactInfo> payloads) : base(payloads)
    {
    }

    protected override async Task<bool> ExecuteActionAsync(LibraryArtefactInfo payload,
        CancellationToken cancellationToken)
    {
        var success = await DoFileAsync(payload, cancellationToken).ConfigureAwait(false);

        return success;
    }

    private static async Task<bool> DoFileAsync(LibraryArtefactInfo artefactInfo,
        CancellationToken cancellationToken)
    {
        try
        {
            var verifyResult = await GameApplication.Libraries.VerifyArtefactAsync(artefactInfo,
                cancellationToken).ConfigureAwait(false);

            if (verifyResult == VerifyResult.NoHash)
            {
                MyOutput.Notice(Smart.Format(Texts.OperationNoticeNoHash,
                    new { artefact = artefactInfo.Path }));
            }
        }
        catch (IOException io)
        {
            MyOutput.Warn(io, Texts.OperationHashFail);
        }

        var target = GameApplication.Libraries.GetArtefactPath(artefactInfo);
        FileHelper.CreateParentDirectory(target, true);
        
        var result = await NetworkHelper.DownloadFileSilentAsync(artefactInfo.Url,
            target,
            cancellationToken
        ).ConfigureAwait(false);
        
        if (!result.IsOk)
        {
            MyOutput.Error(result.ToString());
        }
        else
        {
            AnsiConsole.WriteLine(Texts.RestoreLibraryDownloaded, artefactInfo.Path);
        }

        return result.IsOk;
    }
}