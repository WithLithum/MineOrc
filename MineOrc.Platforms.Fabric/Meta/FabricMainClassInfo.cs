// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Platforms.Fabric.Meta;

public sealed record FabricMainClassInfo
{
    public required string Client { get; init; }
    public required string Server { get; init; }
}