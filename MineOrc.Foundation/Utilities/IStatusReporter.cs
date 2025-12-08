// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Numerics;

namespace MineOrc.Foundation.Utilities;

public interface IStatusReporter
{
    void SetText(string text);
}