// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using JetBrains.Annotations;

namespace MineOrc.Commands;

public static class CommandHelper
{
    public static Option<bool> Switch([LocalizationRequired(false)] string shortName,
        [LocalizationRequired(false)] string longName,
        [LocalizationRequired] string description)
    {
        return new Option<bool>(shortName, longName)
        {
            Description = description,
        };
    }
}