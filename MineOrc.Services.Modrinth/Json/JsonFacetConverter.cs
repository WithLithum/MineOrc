// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using System.Text.Json.Serialization;
using MineOrc.Services.Modrinth.Facets;

namespace MineOrc.Services.Modrinth.Json;

public sealed class JsonFacetConverter : JsonConverter<Facet>
{
    public override Facet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, Facet value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}