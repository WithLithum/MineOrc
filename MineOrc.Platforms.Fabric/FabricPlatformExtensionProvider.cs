// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Instancing;
using MineOrc.Foundation.Platforms;
using MineOrc.Platforms.Fabric.Meta;

namespace MineOrc.Platforms.Fabric;

public sealed class FabricPlatformExtensionProvider : IPlatformExtensionProvider
{
    public async Task<IEnumerable<string>> GetVersionsAsync(string gameVersion,
        HttpClient httpClient,
        CancellationToken cancellationToken = default)
    {
        var loaders = await FabricMetaService.GetSupportedLoadersAsync(gameVersion, httpClient)
            .ConfigureAwait(false);

        return loaders.Select(x => x.Loader.Version);
    }

    public async Task<ProfileExtension> CreateExtensionAsync(string version, string gameVersion,
        HttpClient httpClient,
        CancellationToken cancellationToken = default)
    {
        var meta = await FabricMetaService.GetLoaderAsync(gameVersion,
            version,
            httpClient,
            cancellationToken).ConfigureAwait(false);
        
        return await FabricEvaluator.CreateExtensionOnlineAsync(meta,
            httpClient,
            cancellationToken).ConfigureAwait(false);
    }
}