// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Instancing;

public sealed record ProfileInfo
{
    public required string ClientVersion { get; init; }
    
    public IReadOnlyDictionary<string, ProfileExtension>? Extensions { get; init; }
    
    public required DateTime Created { get; init; }
}