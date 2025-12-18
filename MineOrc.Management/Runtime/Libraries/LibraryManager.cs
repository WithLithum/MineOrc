// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Utilities;
using Owasp.Untrust.BoxedPaths;
using Owasp.Untrust.BoxedPaths.IO;

namespace MineOrc.Management.Runtime.Libraries;

public class LibraryManager
{
    private readonly PathSandbox _pathSandbox;

    public LibraryManager(string rootPath)
    {
        _pathSandbox = PathSandbox.BoxRoot(rootPath);
    }

    public async ValueTask<VerifyResult> VerifyArtefactAsync(LibraryArtefactInfo artefact,
        CancellationToken cancellationToken = default)
    {
        var path = GetArtefactPath(artefact.Path);
        if (artefact.Sha1 == null)
        {
            return BoxedFile.Exists(path)
                ? VerifyResult.NoHash
                : VerifyResult.Damaged;
        }
        
        return await HashHelper.VerifyFileAsync(path, artefact.Sha1, cancellationToken)
            .ConfigureAwait(false)
            ? VerifyResult.Intact
            : VerifyResult.Damaged;
    }

    public BoxedPath GetArtefactPath(LibraryArtefactInfo artefact)
    {
        return GetArtefactPath(artefact.Path);
    }

    private BoxedPath GetArtefactPath(string relativePath)
    {
        return BoxedPath.Of(_pathSandbox, relativePath.Replace('/', Path.DirectorySeparatorChar));
    }
}