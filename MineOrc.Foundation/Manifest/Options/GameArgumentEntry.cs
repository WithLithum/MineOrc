// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MineOrc.Foundation.Json;

namespace MineOrc.Foundation.Manifest.Options;

public sealed record GameArgumentEntry
{
    public IReadOnlyList<GameArgumentRule>? Rules { get; init; }
    
    [JsonConverter(typeof(StringOrStringListConverter))]
    public required IReadOnlyList<string> Value { get; init; }
    
    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator GameArgumentEntry?(string? value)
    {
        if (value == null)
        {
            return null;
        }
        
        return new GameArgumentEntry
        {
            Value = [value]
        };
    }
}