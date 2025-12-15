// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine.Parsing;
using MineOrc.Resources;

namespace MineOrc.Commands;

public static class ArgValidation
{
    public static void NotNegative(ArgumentResult result)
    {
        var value = result.GetValueOrDefault<int>();
        if (value < 0)
        {
            result.AddError(Texts.ArgumentNumberNegative);
        }
    }
    
    public static void NotNegative(OptionResult result)
    {
        var value = result.GetValueOrDefault<int>();
        if (value < 0)
        {
            result.AddError(Texts.ArgumentNumberNegative);
        }
    }
}