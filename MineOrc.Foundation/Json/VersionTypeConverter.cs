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