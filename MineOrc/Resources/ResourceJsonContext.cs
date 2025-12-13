// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json.Serialization;

namespace MineOrc.Resources;

[JsonSerializable(typeof(SecretModel))]
internal partial class ResourceJsonContext : JsonSerializerContext;