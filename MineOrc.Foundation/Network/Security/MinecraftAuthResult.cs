// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;

namespace MineOrc.Foundation.Network.Security;

[Obsolete("Use SessionWithProfile instead.")]
public sealed record MinecraftAuthResult
{
    [MemberNotNullWhen(true, nameof(AccessToken),
        nameof(Profile),
        nameof(Xuid),
        nameof(UserType))]
    public required bool Succeeded { get; init; }
    
    public ProfileExcerpt? Profile { get; init; }
    
    public string? AccessToken { get; init; }
    
    public string? Xuid { get; init; }
    
    public string? UserType { get; init; }
    
    /// <summary>
    /// Gets the value to pass to Minecraft for the user UUID argument instead of the profile
    /// UUID.
    /// </summary>
    public string? OverrideId { get; init; }
}