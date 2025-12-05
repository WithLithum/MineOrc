// Natverk - application server for Minecraft: Java Edition
// Copyright (C) 2025 WithLithum
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Text.Json;
using System.Text.Json.Serialization;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Manifest.Options;

namespace MineOrc.Foundation.Json;

public sealed class JvmArgumentEntryConverter : JsonConverter<JvmArgumentEntry>
{
    public override JvmArgumentEntry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        return reader.TokenType switch
        {
            JsonTokenType.StartObject => JsonSerializer.Deserialize(ref reader,
                VersionManifestDefaultJsonContext.Default.JvmArgumentEntry),
            JsonTokenType.String => ReadString(ref reader),
            JsonTokenType.StartArray => ReadArray(ref reader),
            JsonTokenType.Null => null,
            _ => throw new JsonException($"Expected argument entry, got {reader.TokenType}")
        };
    }

    private static JvmArgumentEntry ReadArray(ref Utf8JsonReader reader)
    {
        var list = new List<string>();
        while (true)
        {
            reader.Read();
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            var str = reader.GetString();
            if (str == null)
            {
                continue;
            }
            
            list.Add(str);
        }

        return new JvmArgumentEntry
        {
            Value = list
        };
    }

    private static JvmArgumentEntry? ReadString(ref Utf8JsonReader reader)
    {
        var str = reader.GetString();

        // Implicit cast do the job
        return str;
    }

    public override void Write(Utf8JsonWriter writer, JvmArgumentEntry value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value,
            VersionManifestDefaultJsonContext.Default.GameArgumentEntry);
    }
}