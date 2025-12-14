// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Utilities.Maven;

namespace MineOrc.Platforms.Fabric.Meta;

public sealed record FabricVersionRef
{
    public required MavenCoordinate Maven { get; init; }
    
    public required string Version { get; init; }
    
    public bool Stable { get; init; }
}