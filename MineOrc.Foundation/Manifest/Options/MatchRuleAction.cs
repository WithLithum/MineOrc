// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;

namespace MineOrc.Foundation.Manifest.Options;

[JsonConverter(typeof(JsonStringEnumConverter<MatchRuleAction>))]
public enum MatchRuleAction
{
    /// <summary>
    /// The argument should be specified.
    /// </summary>
    Allow,
    /// <summary>
    /// The argument should not be specified.
    /// </summary>
    Disallow
}