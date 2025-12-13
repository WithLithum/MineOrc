// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Utilities;

public sealed record ApplicationMeta
{
    public required string Name { get; init; }
    
    public required string Version { get; init; }
    
    /// <summary>
    /// A Java-like package or FreeDesktop scheme name to identify the program.
    /// </summary>
    public required string Package { get; init; }
}