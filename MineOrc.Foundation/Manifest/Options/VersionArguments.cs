// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest.Options;

public sealed record VersionArguments
{
    public required IReadOnlyList<GameArgumentEntry> Game { get; init; }
    
    public required IReadOnlyList<JvmArgumentEntry> Jvm { get; init; }
}