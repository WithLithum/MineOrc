// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using JetBrains.Annotations;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Manifest.Network;
using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Utilities;
using MineOrc.Management.Resources;

namespace MineOrc.Management.Versions;

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
            throw new InvalidOperationException(
                "Manifest path indicates device root which is impossible.");
        }

        Directory.CreateDirectory(parent);
        return File.Create(manifestPath);
    }

    public async Task<ClientManifest> GetManifestAsync(string name,
        CancellationToken cancellationToken = default)
    {
        var stream = File.OpenRead(GetManifestPath(name));
        await using (stream.ConfigureAwait(false))
        {
            return await JsonSerializer.DeserializeAsync(stream,
                           VersionManifestJsonContext.Default.ClientManifest,
                           cancellationToken)
                       .ConfigureAwait(false)
                   ?? throw new InvalidOperationException(ExceptionMessages.ClientManifestNull);
        }
    }

    public async ValueTask<VerifyResult> ValidateJarAsync(string name, ArtefactInfo artefact)
    {
        if (!Exists(name))
        {
            return VerifyResult.Damaged;
        }

        if (artefact.Sha1 == null)
        {
            return VerifyResult.NoHash;
        }

        return await HashHelper.VerifyFileAsync(GetJarPath(name),
            artefact.Sha1).ConfigureAwait(false)
            ? VerifyResult.Intact
            : VerifyResult.Damaged;
    }

    public async ValueTask<bool> ValidateManifestAsync(VersionExcerpt excerpt)
    {
        if (!Exists(excerpt.Id))
        {
            return false;
        }

        return await HashHelper.VerifyFileAsync(GetManifestPath(excerpt.Id),
            excerpt.Sha1).ConfigureAwait(false);
    }

    public bool Exists(string name)
    {
        var manifestPath = GetManifestPath(name);

        return File.Exists(manifestPath);
    }

    public string GetJarPath(string name)
    {
        return Path.Combine(_rootPath, name, $"{name}.jar");
    }

    private string GetManifestPath(string name)
    {
        return Path.Combine(_rootPath, name, $"{name}.json");
    }
}