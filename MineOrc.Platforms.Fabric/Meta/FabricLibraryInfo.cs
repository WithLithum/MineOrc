// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Utilities.Maven;

namespace MineOrc.Platforms.Fabric.Meta;

public sealed record FabricLibraryInfo
{
    public required MavenCoordinate Name { get; init; }
    
    public required Uri Url { get; init; }
    
    public required string Sha1 { get; init; }
    
    public required int Size { get; init; }
}