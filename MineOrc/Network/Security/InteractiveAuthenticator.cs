// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Network.Security;
using MineOrc.Security.Minecraft;
using MineOrc.Security.Xbox;
using MineOrc.UI;
using Spectre.Console;

namespace MineOrc.Network.Security;

public static class InteractiveAuthenticator
{
    public static async Task<PlayAuthSession?> AuthenticateAsync(
        CancellationToken cancellationToken)
    {
        var status = AnsiConsole.Status();

        PlayAuthSession? swp = null;
        try
        {
            await status.StartAsync("Logging in",
                    async c =>
                        swp = await ExecuteLoginAsync(c, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
        }
        catch (XstsException ex)
        {
            MyOutput.Error(ex.Message);
            return null;
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            CommonMsg.ErrorHttpStatus(ex.StatusCode.Value);
        }
        catch (Exception ex) when (ex is HttpRequestException or IOException)
        {
            MyOutput.Error(ex, "Failed to connect");
            return null;
        }

        return swp;
    }

    private static async Task<PlayAuthSession> ExecuteLoginAsync(StatusContext context,
        CancellationToken cancellationToken)
    {
        context.Status = "Logging in with Microsoft Account";
        var msaResult = await MineOrcApp.MsaLogin.SignInWithDeviceCodeAsync(code =>
                {
                    AnsiConsole.MarkupLine("[bold underline yellow]Device Code Login Flow[/]");
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine(
                        "[white]Please open the following URL in your browser:[/]");
                    AnsiConsole.MarkupLineInterpolated(
                        $"[aqua underline]{code.VerificationUrl}[/]");
                    AnsiConsole.MarkupLine("[white]And enter the following code:[/]");
                    AnsiConsole.MarkupLineInterpolated($"[lime]{code.UserCode}[/]");
                    AnsiConsole.WriteLine();

                    context.Status = "Waiting for device login";

                    return Task.CompletedTask;
                },
                cancellationToken)
            .ConfigureAwait(false);

        context.Status = "Logging in with Xbox Live";

        var xboxResult = await XboxLiveService.Authenticate(msaResult.AccessToken,
            MineOrcApp.HttpClient,
            cancellationToken).ConfigureAwait(false);

        context.Status = "Logging in with XSTS";

        var xstsResult = await XstsService.Authenticate(xboxResult.Token,
            MineOrcApp.HttpClient,
            cancellationToken).ConfigureAwait(false);

        context.Status = "Logging in with Minecraft";

        var mcResult = await MinecraftServices.AuthenticateAsync(xstsResult.Token,
            xstsResult.UserHash,
            MineOrcApp.HttpClient,
            cancellationToken).ConfigureAwait(false);

        var profile = await MinecraftServices.GetAuthenticatedProfile(mcResult,
            MineOrcApp.HttpClient).ConfigureAwait(false);
        
        return new PlayAuthSession(profile, mcResult, xstsResult.UserHash);
    }
}