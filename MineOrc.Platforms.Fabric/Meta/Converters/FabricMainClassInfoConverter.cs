// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using System.Text.Json.Serialization;

namespace MineOrc.Platforms.Fabric.Meta.Converters;

public sealed class FabricMainClassInfoConverter : JsonConverter<FabricMainClassInfo>
{
    public override FabricMainClassInfo? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.StartObject => ReadObject(ref reader),
            JsonTokenType.String => ReadString(reader.GetString()),
            _ => throw new JsonException(),
        };
    }

    private static FabricMainClassInfo? ReadString(string? value)
    {
        if (value == null)
        {
            return null;
        }

        return new FabricMainClassInfo
        {
            Client = value,
            Server = value,
        };
    }

    private static FabricMainClassInfo ReadObject(ref Utf8JsonReader reader)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        var client = root.GetProperty("client").GetString();
        var server = root.GetProperty("server").GetString();
        if (string.IsNullOrWhiteSpace(client) || string.IsNullOrWhiteSpace(server))
        {
            throw new JsonException();
        }
        
        return new FabricMainClassInfo
        {
            Client = client,
            Server = server,
        };
    }

    public override void Write(Utf8JsonWriter writer, FabricMainClassInfo value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("client", value.Client);
        writer.WriteString("server", value.Server);
        writer.WriteEndObject();
    }
}