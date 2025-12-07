// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;
using MineOrc.Foundation.Manifest.Options;

namespace MineOrc.Foundation.Json;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(GameArgumentEntry))]
[JsonSerializable(typeof(JvmArgumentEntry))]
[JsonSerializable(typeof(IReadOnlyList<string>), TypeInfoPropertyName = "StringList")]
internal sealed partial class VersionManifestDefaultJsonContext : JsonSerializerContext;