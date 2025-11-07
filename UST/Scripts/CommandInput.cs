using Godot;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text.RegularExpressions;

namespace UST;

public class CommandProcessor {
    private readonly RootCommand RootCommand;
    private readonly Option<string> NameOption = new("--name", "-n") { Required = true, };
    private readonly Option<string> Location1Option = new("--location1", "-l1") { Required = true, };
    private readonly Option<string> Location2Option = new("--location2", "-l2") { Required = true, };

    public CommandProcessor() {
        Command exitCommand = new("exit");
        exitCommand.SetAction(ExitAction);

        Command addPathCommand = new("path") { Options = { this.Location1Option, this.Location2Option, }, };
        addPathCommand.SetAction(this.AddPathAction);

        Command addLocationCommand = new("location") { Options = { this.NameOption, }, };
        addLocationCommand.SetAction(this.AddLocationAction);

        Command addCommand = new("add") { Subcommands = { addLocationCommand, addPathCommand, }, };

        this.RootCommand = new RootCommand("UST CLI") { Subcommands = { addCommand, exitCommand, }, };
    }

    private static void ExitAction(ParseResult _) {
        Main.QuitGame();
    }

    private void AddPathAction(ParseResult pr) {
        GD.Print($"Adding path between {pr.GetValue(this.Location1Option)} and {pr.GetValue(this.Location2Option)}");
    }

    private void AddLocationAction(ParseResult pr) {
        GD.Print($"Adding location with name {pr.GetValue(this.NameOption)}");
    }

    public void Execute(string input) {
        // Test: tater 'tater' 'ta te r' -tater '' --tater -t
        List<string> tokens = Regex.Matches(input, @"('[^']*')|([^' ]+)")
            .Select(x => x.Value)
            .Select(x => x.StartsWith("'", StringComparison.Ordinal) ? x[1..^1] : x)
            .ToList();
        _ = this.RootCommand.Parse(tokens).Invoke();
    }
}
