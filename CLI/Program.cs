﻿
using Microsoft.Extensions.CommandLineUtils;
using CLI.Commands;
using System;
using CLI.Signals;

namespace CLI
{
    public class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
			{
                args = new []{ "watch", "-s" };
			}

            SignalSingleton.ExitSignal.Subscribe(() =>
            {
                Environment.Exit(0);
            });

            var app = new CommandLineApplication();
            app.Name = "ckc";
            app.HelpOption("-?|-h|--help");
            app.VersionOption("-v|--version", "v2.1.4-beta");

            app.OnExecute(() => 0);

            CommandsBuilder.CreateBuildCommand(app);
            CommandsBuilder.CreateWatchCommand(app);
            CommandsBuilder.CreateServeCommand(app);
            try
            {
                app.Execute(args);
            }
            catch (CommandParsingException commandParsingException)
            {
                app.Error.WriteLine("Error: " + commandParsingException.Message);
                app.ShowHelp();
            }
        }
    }
}
