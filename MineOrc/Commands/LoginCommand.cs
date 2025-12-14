// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Resources;
using MineOrc.Security.Minecraft;
using MineOrc.Security.Xbox;
using Spectre.Console;

namespace MineOrc.Commands;

public static class LoginCommand
{
    internal static Command CreateCommand()
    {
        var command = new Command("login");

        command.SetAction(ExecuteAsync);
        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse,
        CancellationToken cancellationToken)
    {
        var status = AnsiConsole.Status();

        try
        {
            await status.StartAsync("Logging in", c => ExecuteLoginAsync(c, cancellationToken))
                .ConfigureAwait(false);
        }
        catch (XstsException ex)
        {
            MyOutput.Error(ex.Message);
            return ExitCodes.Failure;
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            var code = ex.StatusCode.Value;
            MyOutput.Error(Texts.OperationFailHttpError, code.ToString("D"),
                code.ToString("G"));
        }
        catch (Exception ex) when (ex is HttpRequestException or IOException)
        {
            MyOutput.Error(ex, "Failed to connect");
            return ExitCodes.Failure;
        }

        return ExitCodes.Success;
    }

    private static async Task ExecuteLoginAsync(StatusContext context,
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
        
        AnsiConsole.MarkupLineInterpolated($"[green]Welcome back,[/] [yellow]{profile.Name}[/][green]![/]");
    }
}