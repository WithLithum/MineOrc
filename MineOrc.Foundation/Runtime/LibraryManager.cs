// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics;
using JetBrains.Annotations;
using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Manifest.Network;
using MineOrc.Foundation.Utilities;

namespace MineOrc.Foundation.Runtime;

public class LibraryManager
{
    private readonly string _rootPath;

    public LibraryManager(string rootPath)
    {
        _rootPath = rootPath;
    }

    public async ValueTask<bool> VerifyArtefactAsync(LibraryArtefactInfo artefact,
        CancellationToken cancellationToken = default)
    {
        var path = GetArtefactPath(artefact.Path);
        return await HashHelper.VerifyFileAsync(path, artefact.Sha1, cancellationToken)
            .ConfigureAwait(false);
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