// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using System.Text.Json.Serialization;
using MineOrc.Foundation.Utilities.Maven;

namespace MineOrc.Foundation.Json;

public sealed class MavenCoordinateConverter : JsonConverter<MavenCoordinate>
{
    public override MavenCoordinate? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (str == null)
        {
            return null;
        }

        if (!MavenCoordinate.TryParse(str, null, out var result))
        {
            throw new JsonException("Malformed maven coordinates.");
        }
        
        return result;
    }

    public override void Write(Utf8JsonWriter writer, MavenCoordinate value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}