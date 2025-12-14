// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;

namespace MineOrc.Platforms.Fabric.Meta;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(FabricVersionRef))]
[JsonSerializable(typeof(FabricLibraryInfo))]
[JsonSerializable(typeof(FabricLibrariesMeta))]
[JsonSerializable(typeof(FabricLoaderMeta))]
[JsonSerializable(typeof(FabricMainClassInfo))]
[JsonSerializable(typeof(FabricLauncherMeta))]
internal sealed partial class FabricJsonContext : JsonSerializerContext;