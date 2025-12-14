// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;
using MineOrc.Security.Minecraft.Profile;

namespace MineOrc.Security.Minecraft;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(MinecraftServiceAuthResult))]
[JsonSerializable(typeof(MinecraftEntitlementType))]
[JsonSerializable(typeof(MinecraftEntitlement))]
[JsonSerializable(typeof(SecurityProfile))]
internal partial class MinecraftServicesJsonContext : JsonSerializerContext;