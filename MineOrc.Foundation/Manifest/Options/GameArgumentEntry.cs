using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MineOrc.Foundation.Json;

namespace MineOrc.Foundation.Manifest.Options;

[JsonConverter(typeof(GameArgumentEntryConverter))]
public sealed record GameArgumentEntry
{
    public required IReadOnlyList<string> Value { get; init; }
    
    public IReadOnlyList<GameArgumentRule>? Rules { get; init; }
    
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