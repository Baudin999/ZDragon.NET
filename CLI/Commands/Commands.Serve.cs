﻿using System;
using System.IO;
using System.Threading.Tasks;
using CLI.Signals;
using Microsoft.Extensions.CommandLineUtils;
using Project;

namespace CLI.Commands
{
    public partial class CommandsBuilder
    {

        public static void CreateServeCommand(CommandLineApplication app)
        {
            _ = app.Command("serve", (command) =>
            {
                command.Description = "Serve the content of a Project";
                command.HelpOption("-?|-h|--help");

                var fileOption = command.Option(
                      "-f|--file <filePath>",
                      "The path to the file which should be built.",
                      CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    Console.WriteLine(@"
To quit the application press 'q'
");
                    var directory = fileOption.HasValue() switch
                    {
                        false => Directory.GetCurrentDirectory(),
                        true => fileOption.Value()
                    };

                    var project = ProjectContext.Init(directory);
                    SignalSingleton.ExitSignal.Subscribe(project.Dispose);

                    WebServer.Start(project.OutPath);

                    // Wait for the user to quit the program.
                    Console.WriteLine("Press 'q' to quit the sample.");
                    while (Console.Read() != 'q') { }
                    project.Dispose();
                    return 0;
                });
            });
        }


    }
}
