// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Platforms.Fabric.Meta;

public sealed record FabricVersionRef
{
    public required string Maven { get; init; }
    
    public required string Version { get; init; }
    
    public bool Stable { get; init; }
}