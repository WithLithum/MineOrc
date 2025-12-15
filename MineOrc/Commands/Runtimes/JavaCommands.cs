// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using MineOrc.Resources;

namespace MineOrc.Commands.Runtimes;

public static class JavaCommands
{
    internal static async Task<int> SaveChangesAsync()
    {
        // Save our changes
        try
        {
            await MineOrcApp.JavaRegistry.SaveAsync().ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is IOException or JsonException)
        {
            MyOutput.Error(ex, Texts.JavaFailSave);
            return 1;
        }

        return 0;
    }
}