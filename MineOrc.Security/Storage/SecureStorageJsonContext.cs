// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;

namespace MineOrc.Security.Storage;

[JsonSerializable(typeof(SecureStorageKeyRing))]
internal sealed partial class SecureStorageJsonContext : JsonSerializerContext
{
    
}