// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Network.Security;

namespace MineOrc.Security.Minecraft.Profile;

/// <summary>
/// A Minecraft profile acquired through an authenticated request.
/// </summary>
public sealed record SecurityProfile : ISessionProfile
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
    
    public required IReadOnlyList<SecurityProfileTexture> Skins { get; init; }
    
    public required IReadOnlyList<SecurityProfileTexture> Capes { get; init; }
}