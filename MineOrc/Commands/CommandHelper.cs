// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using System.CommandLine.Parsing;
using JetBrains.Annotations;
using MineOrc.Resources;

namespace MineOrc.Commands;

public static class CommandHelper
{
    public static Argument<int> Int32Argument([LocalizationRequired(false)] string name,
        [LocalizationRequired] string description)
    {
        return new Argument<int>(name) { Description = description, };
    }

    public static Argument<string> StringArgument([LocalizationRequired(false)] string name,
        [LocalizationRequired] string description)
    {
        return new Argument<string>(name) { Description = description, };
    }

    public static Argument<FileInfo> FileArgument([LocalizationRequired(false)] string name,
        [LocalizationRequired] string description)
    {
        return new Argument<FileInfo>(name)
        {
            Description = description, Validators = { ValidateFileArgumentInternal },
        };
    }

    private static void ValidateFileArgumentInternal(ArgumentResult state)
    {
        var value = state.GetValueOrDefault<FileInfo>();
        if (!value.Exists)
        {
            state.AddError(Texts.ArgumentFileNotExist);
        }
    }

    public static Option<bool> Switch([LocalizationRequired(false)] string shortName,
        [LocalizationRequired(false)] string longName,
        [LocalizationRequired] string description)
    {
        return new Option<bool>(shortName, longName) { Description = description, };
    }
}