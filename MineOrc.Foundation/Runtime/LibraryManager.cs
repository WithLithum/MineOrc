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

    [MustDisposeResource]
    public Stream CreateArtefact(LibraryArtefactInfo artefact)
    {
        var artefactPath = GetArtefactPath(artefact.Path);
        var artefactDir = Path.GetDirectoryName(artefactPath);
        if (artefactDir == null)
        {
            throw new InvalidOperationException("The artefact parent directory denotes artefact root, which should not be possible.");
        }
        
        Directory.CreateDirectory(artefactDir);
        
        return File.Create(artefactPath);
    }
    
    public async ValueTask<bool> VerifyArtefactAsync(LibraryArtefactInfo artefact,
        CancellationToken cancellationToken = default)
    {
        var path = GetArtefactPath(artefact.Path);
        try
        {
            return await HashHelper.VerifyFileAsync(path, artefact.Sha1, cancellationToken);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    private string GetArtefactPath(string relativePath)
    {
        return Path.GetFullPath(relativePath.Replace('/', Path.DirectorySeparatorChar),
            _rootPath);
    }
}