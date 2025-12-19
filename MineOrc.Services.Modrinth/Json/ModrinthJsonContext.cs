// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;
using MineOrc.Services.Modrinth.Facets;

namespace MineOrc.Services.Modrinth.Json;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(FacetOperator))]
[JsonSerializable(typeof(Facet))]
[JsonSerializable(typeof(FacetGroup), TypeInfoPropertyName = "FacetGroup")]
[JsonSerializable(typeof(FacetSpec), TypeInfoPropertyName = "FacetSpec")]
public sealed partial class ModrinthJsonContext : JsonSerializerContext;