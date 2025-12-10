// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Tests.Utils;

[AttributeUsage(AttributeTargets.Method)]
public sealed class WindowsFact : FactAttribute
{
    public WindowsFact()
    {
        if (!OperatingSystem.IsWindows())
        {
            Skip = "Test is Windows only";
        }
    }
}