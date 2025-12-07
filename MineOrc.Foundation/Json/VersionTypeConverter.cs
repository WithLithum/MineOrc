// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using System.Text.Json.Serialization;
using MineOrc.Foundation.Manifest;

namespace MineOrc.Foundation.Json;

public sealed class VersionTypeConverter : JsonConverter<VersionType>
{
    public override VersionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (str == null)
        {
            throw new JsonException("Expected string but got null");
        }

        return str switch
        {
            "release" => VersionType.Release,
            "snapshot" => VersionType.Snapshot,
            "unobfuscated" => VersionType.Unobfuscated,
            "old_beta" => VersionType.OldBeta,
            "old_alpha" => VersionType.OldAlpha,
            _ => throw new JsonException($"Expected version type, got string '{str}'")
        };
    }

    public override void Write(Utf8JsonWriter writer, VersionType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            VersionType.Release => "release",
            VersionType.Snapshot => "snapshot",
            VersionType.Unobfuscated => "unobfuscated",
            VersionType.OldBeta => "old_beta",
            VersionType.OldAlpha => "old_alpha",
            _ => throw new JsonException($"Unknown version type '{value}'")
        });
    }
}