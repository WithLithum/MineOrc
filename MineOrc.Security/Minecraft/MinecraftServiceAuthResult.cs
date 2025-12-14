// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;
using MineOrc.Foundation.Network.Security;
using MineOrc.Security.Json;

namespace MineOrc.Security.Minecraft;

public sealed record MinecraftServiceAuthResult : IAuthSession
{
    public required string UserName { get; init; }
    
    public required string AccessToken { get; init; }
    
    public required string TokenType { get; init; }

    [JsonConverter(typeof(JsonUtcExpireSecondsConverter))]
    public required DateTimeOffset ExpiresIn { get; init; }
}