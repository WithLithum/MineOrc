// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Platforms.Fabric.Meta;

public sealed record FabricLibrariesMeta
{
    public required IReadOnlyList<FabricLibraryInfo> Common { get; init; }
    
    public required IReadOnlyList<FabricLibraryInfo> Server { get; init; }
    
    public required IReadOnlyList<FabricLibraryInfo> Client { get; init; }
    
    public IReadOnlyList<FabricLibraryInfo>? Development { get; init; }
}