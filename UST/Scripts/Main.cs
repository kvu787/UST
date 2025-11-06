using Godot;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text.RegularExpressions;

namespace UST;

public partial class Main : Node {
    private LineEdit CommandInput;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        Gold gold = new() { Name = "g1", Amount = 2000 };

        this.CommandInput = this.GetNode<LineEdit>("LineEdit");
        this.CommandInput.GuiInput += this.OnCommandInput;

        this.CommandInput.FocusEntered += () => this.CommandInput.PlaceholderText = "";
        this.CommandInput.FocusExited += () => this.CommandInput.PlaceholderText = "Click here. Type a command. Press ENTER to submit.";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    private void OnCommandInput(InputEvent @event) {
        if (@event is InputEventKey keyEvent && keyEvent is { Pressed: true, Keycode: Key.Enter }) {
            string text = this.CommandInput.Text.Trim();
            this.CommandInput.Clear();

            this.HandleCommand(text);
        }
    }

    private void HandleCommand(string command) {
        // Test: tater 'tater' 'ta te r' -tater '' --tater
        List<string> arguments = Regex.Matches(command, @"('[^']*')|([^' ]+)")
            .Select(x => x.Value)
            .Select(x => x.StartsWith("'", StringComparison.Ordinal) ? x[1..^1] : x)
            .ToList();

        foreach (string argument in arguments) {
            GD.Print(argument);
        }

        Option<string> nameOption = new("--name", "-n") { Required = true };
        Command addNodeCommand = new("node") { nameOption };
        addNodeCommand.SetAction(result => {
            GD.Print($"Adding node with name {result.GetValue(nameOption)}");
        });

        Option<string> node1Option = new("--node1", "-n1") { Required = true };
        Option<string> node2Option = new("--node2", "-n2") { Required = true };
        Command addEdgeCommand = new("edge") { node1Option, node2Option };
        addEdgeCommand.SetAction(result => {
            GD.Print($"Adding edge between {result.GetValue(node1Option)} and {result.GetValue(node2Option)}");
        });

        Command exitCommand = new("exit");
        exitCommand.SetAction(_ => this.GetTree().Quit());

        RootCommand rootCommand = new("UST CLI") {
            new Command("add") {
                addNodeCommand,
                addEdgeCommand,
            },
            exitCommand,
        };

        _ = rootCommand.Parse(arguments).Invoke();

        // add unit --template <string> --name <string>
        // add building --template <string> --name <string>
    }
}
