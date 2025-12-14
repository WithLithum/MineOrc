// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Security.Minecraft.Profile;

public sealed record SecurityProfileTexture
{
    public required Guid Id { get; init; }
    
    public required string State { get; init; }
    
    public required Uri Url { get; init; }
    
    public required string Variant { get; init; }
}