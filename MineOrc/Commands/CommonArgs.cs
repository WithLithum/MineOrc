// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Resources;

namespace MineOrc.Commands;

public static class CommonArgs
{
    public static readonly Option<int> SearchLimit = CommandHelper.PositiveInt32Option("-l",
        "--limit",
        Texts.CommandGenericOptionSearchLimit);
}