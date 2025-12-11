// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Instancing;

public class ProfileException : Exception
{
    public ProfileException()
    {
    }

    public ProfileException(string? message) : base(message)
    {
    }

    public ProfileException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}