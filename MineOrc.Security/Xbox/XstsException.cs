// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Security.Resources;

namespace MineOrc.Security.Xbox;

public class XstsException : Exception
{
    public XstsException()
    {
    }

    public XstsException(string? message) : base(message)
    {
    }

    public XstsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static XstsException CreateFromCode(long code)
    {
        var message = code switch
        {
            2148916227 => Messages.XstsErrorAccountBanned,
            2148916233 => Messages.XstsErrorNoXboxAccount,
            2148916235 => Messages.XstsErrorUnavailableRegion,
            2148916236 or 2148916237 => Messages.XstsErrorAdultVerification,
            2148916238 => Messages.XstsErrorAddChildToFamily,
            _ => Messages.FormatXstsErrorUnknown(code),
        };

        return new XstsException(message);
    }
}