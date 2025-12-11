// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;
using MineOrc.Foundation.Instancing;
using MineOrc.Foundation.Runtime.Java;

namespace MineOrc.Foundation.Json;

[JsonSourceGenerationOptions(WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(JavaInfo))]
[JsonSerializable(typeof(IReadOnlyList<JavaInfo>))]
[JsonSerializable(typeof(IReadOnlyDictionary<string, JavaInfo>))]
[JsonSerializable(typeof(JavaData))]
[JsonSerializable(typeof(ProfileInfo))]
public sealed partial class MineOrcFilesJsonContext : JsonSerializerContext;