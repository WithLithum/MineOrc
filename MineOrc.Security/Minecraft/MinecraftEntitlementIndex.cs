// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;

namespace MineOrc.Security.Minecraft;

public class MinecraftEntitlementIndex
{
    public required IReadOnlyList<MinecraftEntitlement> Entitlements { get; init; }
    
    public required string Signature { get; init; }
    
    [JsonPropertyName("keyId")]
    public required int KeyId { get; init; }
}