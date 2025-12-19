// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using LinkDotNet.StringBuilder;
using MineOrc.Services.Modrinth.Json;

namespace MineOrc.Services.Modrinth.Facets;

[JsonConverter(typeof(JsonFacetConverter))]
public sealed record Facet
{
    public Facet()
    {
    }

    [SetsRequiredMembers]
    public Facet(string property, FacetOperator facetOperator, string value)
    {
        Property = property;
        Operator = facetOperator;
        Value = value;
    }

    public required string Property { get; init; }

    public required FacetOperator Operator { get; init; }

    public required string Value { get; init; }

    public override string ToString()
    {
        using var vsb = new ValueStringBuilder();
        vsb.Append(Property);
        vsb.Append(Operator.AsString());
        vsb.Append(Value);
        return vsb.ToString();
    }
    
    public static Facet MatchDate(string property,
        FacetOperator @operator,
        DateTimeOffset value)
    {
        return new Facet(property, @operator, value.ToString("O"));
    }
}