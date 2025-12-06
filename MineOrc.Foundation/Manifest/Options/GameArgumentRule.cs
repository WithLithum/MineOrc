// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Options;

public sealed record GameArgumentRule
{
    /// <summary>
    /// Gets the action to take if the conditions of this rule was satisfied.
    /// </summary>
    public required MatchRuleAction Action { get; init; }
    
    /// <summary>
    /// Gets the options to allow or disallow for this argument rule.
    /// </summary>
    /// <seealso cref="GameArgumentFeature"/>
    public IReadOnlyDictionary<string, bool>? Features { get; init; }
}