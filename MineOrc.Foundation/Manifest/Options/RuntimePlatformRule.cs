// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Options;

public sealed record RuntimePlatformRule
{
    public string? Name { get; init; }
    
    public string? Version { get; init; }
    
    public string? Arch { get; init; }
}