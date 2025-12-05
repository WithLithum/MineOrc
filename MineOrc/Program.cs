using MineOrc;
using MineOrc.Commands;
using MineOrc.Resources;
using Spectre.Console.Cli;

var commandApp = new CommandApp();
commandApp.Configure(root =>
{
    root.SetApplicationName(MineOrcApp.BaseName);
    root.PropagateExceptions();

    root.AddBranch("version", client =>
    {
        client.SetDescription(Messages.VersionBranchDescription!);
        client.AddCommand<SearchVersionCommand>("search");
    });
});

return await commandApp.RunAsync(args);