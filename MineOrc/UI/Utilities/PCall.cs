// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Resources;

namespace MineOrc.UI.Utilities;

public static class PCall
{
    public static async Task<bool> LocalIoAsync(Func<Task> action)
    {
        try
        {
            await action().ConfigureAwait(false);
        }
        catch (UnauthorizedAccessException ex)
        {
            MyOutput.Error(Texts.FormatOperationGenericIoDenied(ex.Message));
            return false;
        }
        catch (IOException ex)
        {
            MyOutput.Error(ex, Texts.OperationGenericExceptionError);
            return false;
        }

        return true;
    }

    public static async Task<bool> NetworkAsync(Func<Task> action)
    {
        try
        {
            await action().ConfigureAwait(false);
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            MyOutput.Error(Texts.FormatOperationGenericFailHttpStatus(
                code: ex.StatusCode.Value.ToString("D"),
                text: ex.StatusCode.Value.ToString("G")
            ));
            return false;
        }
        catch (HttpRequestException ex)
        {
            MyOutput.Error(Texts.FormatOperationGenericFailNetwork(ex.Message));
            return false;
        }
        catch (IOException ex)
        {
            MyOutput.Error(ex, Texts.OperationGenericExceptionError);
            return false;
        }

        return true;
    }
}