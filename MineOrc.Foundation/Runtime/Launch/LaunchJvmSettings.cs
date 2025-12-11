// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Runtime.Java;

namespace MineOrc.Foundation.Runtime.Launch;

public sealed record LaunchJvmSettings
{
    public required string MainClass { get; init; }
    
    public required string NativesDirectory { get; init; }
    
    public required string LauncherBrand { get; init; }
    
    public required string LauncherVersion { get; init; }
    
    public int? MinMemory { get; init; }
    
    public int? MaxMemory { get; init; }
}