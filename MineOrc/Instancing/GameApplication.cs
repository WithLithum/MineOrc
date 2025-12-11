// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Utilities;
using MineOrc.Management.Runtime.Assets;
using MineOrc.Management.Runtime.Libraries;
using MineOrc.Management.Versions;

namespace MineOrc.Instancing;

public static class GameApplication
{
    public static AssetManager Assets { get; } = new(MinecraftDirectory.UserAssets);
    
    public static LibraryManager Libraries { get; } = new(MinecraftDirectory.UserLibraries);
    
    public static VersionManager Versions { get; } = new(MinecraftDirectory.UserVersions);
}