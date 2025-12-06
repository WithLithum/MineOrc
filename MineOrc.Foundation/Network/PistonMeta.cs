// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net.Http.Json;
using MineOrc.Foundation.Manifest;

namespace MineOrc.Foundation.Network;

public class PistonMeta
{
    private static readonly Uri DefaultUrlRoot = new("https://piston-meta.mojang.com/");
    private const string VersionManifestV2 = "/mc/game/version_manifest_v2.json";
    
    private readonly HttpClient _client;
    private readonly Uri _urlRoot;

    public PistonMeta(HttpClient client, Uri? urlRoot = null)
    {
        _client = client;
        _urlRoot = urlRoot ?? DefaultUrlRoot;
    }

    public async Task<VersionManifest> GetVersionManifest(CancellationToken cancellationToken = default)
    {
        var manifestUrl = new Uri(_urlRoot, VersionManifestV2);

        return await _client.GetFromJsonAsync(manifestUrl,
            VersionManifestJsonContext.Default.VersionManifest,
            cancellationToken)
            ?? throw new InvalidOperationException($"Failed to get manifest from url: {manifestUrl}");
    }
}