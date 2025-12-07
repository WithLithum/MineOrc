// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Network;

public static class NetworkGlobals
{
    public const int SmallBufferSize = 32;

    public const int MediumBufferSize = 256;
    public const int MediumBufferThreshold = 1024;

    public const int LargeBufferSize = 1024;
    public const int LargeBufferThreshold = LargeBufferSize * 10;
}