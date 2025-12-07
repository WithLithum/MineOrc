// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Runtime.Resources;
using MineOrc.Foundation.Utilities;

namespace MineOrc.Instancing;

public static class GameApplication
{
    public static AssetManager Assets { get; } = new(MinecraftDirectory.UserAssets);
}