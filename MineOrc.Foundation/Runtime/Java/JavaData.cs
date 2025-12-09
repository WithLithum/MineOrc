// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Runtime.Java;

public record JavaData(string? Default,
    IReadOnlyDictionary<string, JavaInfo> Runtimes);