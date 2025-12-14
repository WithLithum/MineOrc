// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MineOrc.Foundation.Json;

namespace MineOrc.Foundation.Utilities.Maven;

[JsonConverter(typeof(MavenCoordinateConverter))]
public sealed record MavenCoordinate : ISpanParsable<MavenCoordinate>
{
    [JsonConstructor]
    public MavenCoordinate()
    {
    }

    [SetsRequiredMembers]
    public MavenCoordinate(string group, string artefact, string version)
    {
        Group = group;
        Artefact = artefact;
        Version = version;
    }

    public required string Group { get; init; }

    public required string Artefact { get; init; }

    public required string Version { get; init; }

    public static MavenCoordinate Parse(string s)
        => Parse(s, null);

    public static MavenCoordinate Parse(ReadOnlySpan<char> s)
        => Parse(s, null);

    public static MavenCoordinate Parse(string s, IFormatProvider? provider)
    {
        return Parse(s.AsSpan(), provider);
    }

    public static MavenCoordinate Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            throw new FormatException();
        }

        return result;
    }

    public static bool TryParse(string s, [MaybeNullWhen(false)] out MavenCoordinate result)
        => TryParse(s, null, out result);

    public static bool TryParse(ReadOnlySpan<char> s,
        [MaybeNullWhen(false)] out MavenCoordinate result)
        => TryParse(s, null, out result);

    public static bool TryParse([NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out MavenCoordinate result)
    {
        if (s == null)
        {
            result = null;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        [MaybeNullWhen(false)] out MavenCoordinate result)
    {
        Span<Range> ranges = stackalloc Range[4];
        if (s.Split(ranges, ':') != 3)
        {
            result = null;
            return false;
        }

        var groupPart = s[ranges[0]];
        var artefactPart = s[ranges[1]];
        var versionPart = s[ranges[2]];

        result = new MavenCoordinate(groupPart.ToString(),
            artefactPart.ToString(),
            versionPart.ToString());
        return true;
    }

    public override string ToString()
    {
        return $"{Group}:{Artefact}:{Version}";
    }
}