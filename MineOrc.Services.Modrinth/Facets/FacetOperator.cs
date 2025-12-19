// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Services.Modrinth.Facets;

/// <summary>
/// Specifies the operator in the facet.
/// </summary>
public enum FacetOperator
{
    /// <summary>
    /// The <c>=</c> or <c>:</c> operator, which matches projects with value of the specified
    /// property equal to the specified value.
    /// </summary>
    Equal,
    /// <summary>
    /// The <c>!=</c> operator, which matches projects with value of the specified property
    /// different to the specified value. 
    /// </summary>
    NotEqual,
    /// <summary>
    /// The <c>></c> operator, which matches projects with value of the specified property greater
    /// than the specified value.
    /// </summary>
    Greater,
    /// <summary>
    /// The <c>></c> operator, which matches projects with value of the specified property greater
    /// than or equal to the specified value.
    /// </summary>
    GreaterOrEqual,
    /// <summary>
    /// The <c>></c> operator, which matches projects with value of the specified property less
    /// than the specified value.
    /// </summary>
    Less,
    /// <summary>
    /// The <c>></c> operator, which matches projects with value of the specified property less
    /// than or equal to the specified value.
    /// </summary>
    LessOrEqual,
}