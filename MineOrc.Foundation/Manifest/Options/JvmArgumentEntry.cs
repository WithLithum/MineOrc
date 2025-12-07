// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MineOrc.Foundation.Json;

namespace MineOrc.Foundation.Manifest.Options;

public sealed record JvmArgumentEntry
{
    public IReadOnlyList<RuntimeRule>? Rules { get; init; }
 
    [JsonConverter(typeof(StringOrStringListConverter))]
    public required IReadOnlyList<string> Value { get; init; }

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator JvmArgumentEntry?(string? value)
    {
        if (value == null)
        {
            return null;
        }
        
        return new JvmArgumentEntry
        {
            Value = [value]
        };
    }
}