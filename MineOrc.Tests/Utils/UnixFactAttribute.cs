// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Tests.Utils;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnixFactAttribute : FactAttribute
{
    public UnixFactAttribute()
    {
        if (Environment.OSVersion.Platform != PlatformID.Unix)
        {
            Skip = "Test is Unix only";
        }
    }
}