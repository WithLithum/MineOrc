// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Manifest;

public sealed record RuntimeVersionInfo
{
    public required string Component { get; init; }
    
    public required int MajorVersion { get; init; }
}