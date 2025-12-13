// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.IO;

namespace Build;

public static class CommonPaths
{
    internal static readonly string Solution = Path.GetFullPath("MineOrc.slnx",
        Path.GetDirectoryName(Environment.CurrentDirectory)
        ?? Environment.CurrentDirectory);
}