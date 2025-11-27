using Godot;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace UST;

public partial class Terminal {
    private readonly Argument<string> NameArgument = new("name");

    private readonly Option<string> NameOption = new("--name", "-n") { Required = true };
    private readonly Option<string> LocationOption = new("--location", "-l") { Required = true };
    private readonly Option<string> Location1Option = new("--location1", "-l1") { Required = true };
    private readonly Option<string> Location2Option = new("--location2", "-l2") { Required = true };
    private readonly Option<string> IdOption = new("--id", "-i") { Required = true };

    private readonly RootCommand RootCommand = new("UST CLI");

    private readonly Command AddCommand = new("add");
    private readonly Command AddLocationCommand = new("location");
    private readonly Command MoveCommand = new("move");
    private readonly Command AddPathCommand = new("path");
    private readonly Command ExitCommand = new("exit");
    private readonly Command OpenCommand = new("open");

    private Node Node;
    private LineEdit LineEdit;

    public Terminal(Node node, LineEdit lineEdit) {
        this.Node = node;
        this.LineEdit = lineEdit;
        this.SetupHierarchy();
        this.SetupArgumentsAndOptions();
        this.SetupActions();

        this.LineEdit.GuiInput += this.OnTerminalSubmit;
        this.LineEdit.FocusEntered += () => this.LineEdit.PlaceholderText = "";
        this.LineEdit.FocusExited += () => this.LineEdit.PlaceholderText = "Click here. Type a command. Press ENTER to submit.";
    }

    private void OnTerminalSubmit(InputEvent @event) {
        if (@event is InputEventKey keyEvent && keyEvent is { Pressed: true, Keycode: Key.Enter }) {
            this.Execute(this.LineEdit.Text.Trim());
            this.LineEdit.Clear();
        }
    }

    private void SetupHierarchy() {
        this.RootCommand.Subcommands.Add(this.AddCommand);
        {
            this.AddCommand.Subcommands.Add(this.AddPathCommand);
            this.AddCommand.Subcommands.Add(this.AddLocationCommand);
        }
        this.RootCommand.Subcommands.Add(this.ExitCommand);
        this.RootCommand.Subcommands.Add(this.OpenCommand);
    }

    private void SetupArgumentsAndOptions() {
        this.AddLocationCommand.Options.Add(this.NameOption);
        this.AddPathCommand.Options.Add(this.Location1Option);
        this.AddPathCommand.Options.Add(this.Location2Option);
        this.MoveCommand.Options.Add(this.IdOption);
        this.MoveCommand.Options.Add(this.NameOption);
        this.OpenCommand.Arguments.Add(this.NameArgument);
    }

    private void SetupActions() {
        this.AddLocationCommand.SetAction(this.AddLocationAction);
        this.AddPathCommand.SetAction(this.AddPathAction);
        this.ExitCommand.SetAction(this.ExitAction);
        this.OpenCommand.SetAction(this.OpenAction);
    }

    private void OpenAction(ParseResult pr) {
        string fileName = pr.GetValue(this.NameArgument);
        if (fileName.IndexOfAny(['/', '\\']) != -1) {
            GD.Print($"Error: File name must not contain slashes. File name: {fileName}.");
            return;
        }

        string filePath = Path.Combine(ProjectSettings.GlobalizePath("user://"), fileName);
        if (!File.Exists(filePath)) {
            GD.Print($"Creating new file at '{filePath}' ...");
            using FileStream _ = File.Create(filePath);
        }

        Globals.SaveFileConnected = true;
    }

    private void ExitAction(ParseResult _) {
        this.Node.GetTree().Quit();
    }

    private void AddPathAction(ParseResult pr) {
        GD.Print($"Adding path between {pr.GetValue(this.Location1Option)} and {pr.GetValue(this.Location2Option)}");
    }

    private void AddLocationAction(ParseResult pr) {
        GD.Print($"Adding location with name {pr.GetValue(this.NameOption)}");
    }

    public void Execute(string input) {
        // Test: tater 'tater' 'ta te r' -tater '' --tater -t
        List<string> tokens = TokenRegex().Matches(input)
            .Select(x => x.Value)
            .Select(x => x.StartsWith('\'') ? x[1..^1] : x)
            .ToList();

        ParseResult parseResult = this.RootCommand.Parse(tokens);
        if (!Globals.SaveFileConnected
            && parseResult.CommandResult.Command != this.ExitCommand
            && parseResult.CommandResult.Command != this.OpenCommand) {
            GD.Print("You must open a save file before executing other commands.");
            return;
        }

        _ = parseResult.Invoke();
    }

    [GeneratedRegex(@"('[^']*')|([^' ]+)")]
    private static partial Regex TokenRegex();
}
