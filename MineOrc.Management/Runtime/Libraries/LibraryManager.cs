// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Utilities;

namespace MineOrc.Management.Runtime.Libraries;

public class LibraryManager
{
    private readonly string _rootPath;

    public LibraryManager(string rootPath)
    {
        _rootPath = rootPath;
    }

    public async ValueTask<VerifyResult> VerifyArtefactAsync(LibraryArtefactInfo artefact,
        CancellationToken cancellationToken = default)
    {
        var path = GetArtefactPath(artefact.Path);
        if (artefact.Sha1 == null)
        {
            return File.Exists(path)
                ? VerifyResult.NoHash
                : VerifyResult.Damaged;
        }
        
        return await HashHelper.VerifyFileAsync(path, artefact.Sha1, cancellationToken)
            .ConfigureAwait(false)
            ? VerifyResult.Intact
            : VerifyResult.Damaged;
    }

    public string GetArtefactPath(LibraryArtefactInfo artefact)
    {
        return GetArtefactPath(artefact.Path);
    }

    private string GetArtefactPath(string relativePath)
    {
        return Path.GetFullPath(relativePath.Replace('/', Path.DirectorySeparatorChar),
            _rootPath);
    }
}