// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Runtime.Launch;

public record LaunchJvmSettings(int? MinMemory,
    int MaxMemory);