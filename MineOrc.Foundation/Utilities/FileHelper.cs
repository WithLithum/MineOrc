// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Utilities;

public static class FileHelper
{
    public static void CreateParentDirectory(string filePath, bool noRoot)
    {
        var parent = Path.GetDirectoryName(filePath);
        if (parent == null)
        {
            if (noRoot)
            {
                throw new InvalidOperationException("The parent path indicates a root directory which is prohibited.");
            }

            return;
        }

        Directory.CreateDirectory(parent);
    }
}