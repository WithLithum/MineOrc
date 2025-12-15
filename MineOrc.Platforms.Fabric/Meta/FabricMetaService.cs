// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net.Http.Json;

namespace MineOrc.Platforms.Fabric.Meta;

public static class FabricMetaService
{
    public static async Task<IReadOnlyList<FabricLoaderMeta>> GetSupportedLoadersAsync(string versionId,
        HttpClient client)
    {
        var url = $"https://meta.fabricmc.net/v2/versions/loader/{Uri.EscapeDataString(versionId)}";

        return (await client.GetFromJsonAsync(url,
                FabricJsonContext.Default.IReadOnlyListFabricLoaderMeta)
            .ConfigureAwait(false))!;
    }

    public static async Task<FabricLoaderMeta> GetLoaderAsync(string gameVersion,
        string loaderVersion,
        HttpClient client,
        CancellationToken cancellationToken = default)
    {
        var url = $"https://meta.fabricmc.net/v2/versions/loader/{Uri.EscapeDataString(gameVersion)}" +
                  $"/{Uri.EscapeDataString(loaderVersion)}";

        return (await client.GetFromJsonAsync(url,
                FabricJsonContext.Default.FabricLoaderMeta,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false))!;
    }
}