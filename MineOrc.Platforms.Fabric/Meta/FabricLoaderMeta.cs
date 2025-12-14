// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Platforms.Fabric.Meta;

public sealed record FabricLoaderMeta
{
    public required FabricVersionRef Loader { get; init; }
    
    public required FabricVersionRef Intermediary { get; init; }
}