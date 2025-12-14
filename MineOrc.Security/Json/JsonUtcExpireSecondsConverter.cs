// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using System.Text.Json.Serialization;

namespace MineOrc.Security.Json;

public sealed class JsonUtcExpireSecondsConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt64();
        return DateTimeOffset.UtcNow.AddSeconds(value);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        var now = DateTimeOffset.UtcNow;
        if (value > now)
        {
            writer.WriteNumberValue(0);
        }
        else
        {
            var difference = value - now;
            writer.WriteNumberValue(difference.TotalSeconds);
        }
    }
}