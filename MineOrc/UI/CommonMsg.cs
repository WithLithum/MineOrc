// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net;
using MineOrc.Resources;

namespace MineOrc.UI;

internal static class CommonMsg
{
    internal static void ErrorNoProfile(string profileName)
    {
        MyOutput.Error(Texts.FormatCommandGenericNoProfile(profileName));
    }

    internal static void ErrorHttpStatus(HttpStatusCode statusCode)
    {
        MyOutput.Error(Texts.FormatOperationFailHttpError(
            statusCode.ToString("D"),
            statusCode.ToString("G")));
    }
}