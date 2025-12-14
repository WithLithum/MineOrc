// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Network.Security;

namespace MineOrc.Security.Minecraft.Profile;

public sealed record SecurityAuthDetail : IAuthDetail
{
    public required string XboxUserHash { get; init; }
}