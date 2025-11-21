using Godot;
using System.Collections.Generic;

namespace UST;

public partial class Main : Node {
    private LineEdit TerminalLineEdit;
    private Terminal Terminal;

    public static bool SaveFileConnected { get; set; }

    // Game data
    private Dictionary<int, HashSet<int>> map;
    private List<int> teams;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        GD.Print("Ready");
        GD.Print($"Game data folder: {ProjectSettings.GlobalizePath("user://")}");

        this.SetupTerminal();
        this.SetupGameData();
    }

    private void SetupGameData() {
        /*
        Generate the graph
        Generate mapping of graph vertices to team ownership (1 or 2)
        Generate units
        Place units on map
        */
        this.map = Graph.GenerateMap(10, 0.2);
        this.teams = Graph.GenerateTeamMap(this.map, 0.65);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        /*
        Loop through and execute each unit's automaton
        if win condition is met, exit loop and print results
        */
    }

    private void SetupTerminal() {
        this.Terminal = new Terminal(this);
        this.TerminalLineEdit = this.GetNode<LineEdit>("LineEdit");
        this.TerminalLineEdit.GuiInput += this.OnTerminalSubmit;
        this.TerminalLineEdit.FocusEntered += () => this.TerminalLineEdit.PlaceholderText = "";
        this.TerminalLineEdit.FocusExited += () => this.TerminalLineEdit.PlaceholderText = "Click here. Type a command. Press ENTER to submit.";
    }

    private void OnTerminalSubmit(InputEvent @event) {
        if (@event is InputEventKey keyEvent && keyEvent is { Pressed: true, Keycode: Key.Enter }) {
            this.Terminal.Execute(this.TerminalLineEdit.Text.Trim());
            this.TerminalLineEdit.Clear();
        }
    }
}
