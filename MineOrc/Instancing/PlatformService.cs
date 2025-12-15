// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Platforms;
using MineOrc.Platforms.Fabric;

namespace MineOrc.Instancing;

public static class PlatformService
{
    public static readonly IReadOnlyDictionary<string, IPlatformExtensionProvider>
        ExtensionProviders =
            new Dictionary<string, IPlatformExtensionProvider>
            {
                { "fabric", new FabricPlatformExtensionProvider() },
            };
}