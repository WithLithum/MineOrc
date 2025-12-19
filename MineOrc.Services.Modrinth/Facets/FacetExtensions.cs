// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Services.Modrinth.Facets;

public static class FacetExtensions
{
    extension(FacetOperator @operator)
    {
        public string AsString()
        {
            return @operator switch
            {
                FacetOperator.Equal => "=",
                FacetOperator.NotEqual => "!=",
                FacetOperator.Greater => ">",
                FacetOperator.GreaterOrEqual => ">=",
                FacetOperator.Less => "<",
                FacetOperator.LessOrEqual => "<=",
                _ => throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null),
            };
        }
    }
}