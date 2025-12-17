// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Runtime.Java;

namespace MineOrc.Management.Runtime.Java;

public interface IJavaLocator
{
    bool CanExecute();
    
    IEnumerable<KeyValuePair<string, JavaInfo>> SearchJava();
}