// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Platforms.Fabric.Meta;

public sealed record FabricLauncherMeta
{
    public required FabricLibrariesMeta Libraries { get; init; }
    
    public required FabricMainClassInfo MainClass { get; init; }
}