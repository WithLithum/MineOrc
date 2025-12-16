// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;

namespace MineOrc.Foundation.Utilities.Maven;

public sealed partial record MavenCoordinate
{
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
        Span<Range> ranges = stackalloc Range[5];
        var count = s.Split(ranges, ':');
        result = null;
        switch (count)
        {
            case 3:
                result = AssembleWithNoClassifier(s, ranges);
                return true;
            case 4:
                result = AssembleWithClassifier(s, ranges);
                return true;
            default:
                return false;
        }
    }

    private static MavenCoordinate AssembleWithNoClassifier(ReadOnlySpan<char> s,
        Span<Range> ranges)
    {
        var groupPart = s[ranges[0]];
        var artefactPart = s[ranges[1]];
        var versionPart = s[ranges[2]];

        return new MavenCoordinate(groupPart.ToString(),
            artefactPart.ToString(),
            versionPart.ToString());
    }

    private static MavenCoordinate AssembleWithClassifier(ReadOnlySpan<char> s,
        Span<Range> ranges)
    {
        var groupPart = s[ranges[0]];
        var artefactPart = s[ranges[1]];
        var versionPart = s[ranges[2]];
        var classifierPart = s[ranges[3]];

        return new MavenCoordinate(groupPart.ToString(),
            artefactPart.ToString(),
            versionPart.ToString(),
            classifierPart.ToString());
    }
}