// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;

namespace MineOrc.Foundation.Runtime.Java;

public record JavaInfo
{
    public JavaInfo()
    {
    }
    
    [SetsRequiredMembers]
    public JavaInfo(int majorVersion,
        string executablePath,
        bool autoAdded = false)
    {
        MajorVersion = majorVersion;
        ExecutablePath = executablePath;
        AutoAdded = autoAdded;
    }

    public required int MajorVersion { get; init; }
    public required string ExecutablePath { get; init; }

    public bool AutoAdded { get; init; }
}