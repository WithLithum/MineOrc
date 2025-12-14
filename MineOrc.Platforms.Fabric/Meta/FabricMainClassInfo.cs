// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;
using MineOrc.Platforms.Fabric.Meta.Converters;

namespace MineOrc.Platforms.Fabric.Meta;

[JsonConverter(typeof(FabricMainClassInfoConverter))]
public sealed record FabricMainClassInfo
{
    public required string Client { get; init; }
    public required string Server { get; init; }
}