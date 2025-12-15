// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Manifest.Options;

namespace MineOrc.Foundation.Instancing;

public sealed record ProfileExtension
{
    public string? Version { get; init; }
    
    public IReadOnlyList<LibraryInfo>? Libraries { get; init; }
    
    public IReadOnlyList<GameArgumentEntry>? GameArguments { get; init; }
    
    public IReadOnlyList<JvmArgumentEntry>? JvmArguments { get; init; }
    
    public string? MainClass { get; init; }
}