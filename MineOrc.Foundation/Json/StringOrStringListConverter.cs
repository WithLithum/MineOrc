// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using System.Text.Json.Serialization;

namespace MineOrc.Foundation.Json;

public class StringOrStringListConverter : JsonConverter<IReadOnlyList<string>>
{
    public override IReadOnlyList<string>? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => [reader.GetString()!],
            JsonTokenType.Null => null,
            _ => JsonSerializer.Deserialize(ref reader, VersionManifestDefaultJsonContext.Default.StringList)
        };
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<string> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}