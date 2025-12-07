// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using JetBrains.Annotations;
using MineOrc.Foundation.Manifest;

namespace MineOrc.Foundation.Runtime;

public class VersionManager
{
    private readonly string _rootPath;

    public VersionManager(string rootPath)
    {
        _rootPath = rootPath;
    }

    [MustDisposeResource]
    public Stream CreateManifest(string name)
    {
        var manifestPath = GetManifestPath(name);
        var parent = Path.GetDirectoryName(manifestPath);
        if (parent == null)
        {
            throw new InvalidOperationException("Manifest path indicates device root which is impossible.");
        }
        
        Directory.CreateDirectory(parent);
        return File.Create(manifestPath);
    }
    
    public async Task<ClientManifest> GetManifestAsync(string name)
    {
        var stream = File.OpenRead(GetManifestPath(name));
        await using (stream.ConfigureAwait(false))
        {
            return JsonSerializer.Deserialize(stream,
                VersionManifestJsonContext.Default.ClientManifest)
                ?? throw new InvalidOperationException("The client manifest is literal null.");
        }
    }
    
    public bool Exists(string name)
    {
        var manifestPath = GetManifestPath(name);
        
        return File.Exists(manifestPath);
    }

    private string GetManifestPath(string name)
    {
        return Path.Combine(_rootPath, "name", $"{name}.json");
    }
}