// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;

namespace MineOrc.Network.Security;

public class AccountManager
{
    // This is a non-functioning API stub used for quick and dirty prototyping to get demo launch
    // working.
    
    public string? DefaultAccount { get; set; }

    public bool TryGetAccount(string accountName,
        [NotNullWhen(true)] out IAccountModel? result)
    {
        // TODO implement
        result = null;
        return false;
    }
}