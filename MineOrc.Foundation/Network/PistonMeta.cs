// Natverk - application server for Minecraft: Java Edition
// Copyright (C) 2025 WithLithum
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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