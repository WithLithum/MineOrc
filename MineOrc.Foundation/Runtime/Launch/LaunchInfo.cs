// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Network.Security;
using MineOrc.Foundation.Runtime.Java;

namespace MineOrc.Foundation.Runtime.Launch;

public sealed record LaunchInfo
{
    public required IReadOnlyList<string> Classpath { get; init; }
    
    public required AuthenticationResult AuthenticationResult { get; init; }
    
    public required string AssetsVersion { get; init; }
    
    public required string AssetsRoot { get; init; }
    
    public required string VersionName { get; init; }
    
    public required string MainClass { get; init; }
    
    public required JavaInfo Java { get; init; }
    
    public required LaunchJvmSettings JvmSettings { get; init; }
}