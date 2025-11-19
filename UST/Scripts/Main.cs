using Godot;
using System;
using System.Collections.Generic;

namespace UST;

public partial class Main : Node {
    private static Main MainInstance;
    private LineEdit TerminalLineEdit;
    private Terminal Terminal;

    public static bool SaveFileConnected { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        GD.Print("Ready");
        GD.Print($"Game data folder: {ProjectSettings.GlobalizePath("user://")}");

        MainInstance = this;
        this.Terminal = new Terminal();

        this.TerminalLineEdit = this.GetNode<LineEdit>("LineEdit");
        this.TerminalLineEdit.GuiInput += this.OnTerminalSubmit;

        this.TerminalLineEdit.FocusEntered += () => this.TerminalLineEdit.PlaceholderText = "";
        this.TerminalLineEdit.FocusExited += () => this.TerminalLineEdit.PlaceholderText = "Click here. Type a command. Press ENTER to submit.";
    }

    private static Dictionary<int, HashSet<int>> GenerateMap(int numLocations, (int, int) numConnectionsRange) {
        Dictionary<int, HashSet<int>> map = [];
        Random random = new();

        for (int i = 0; i < numLocations; i++) {
            map.Add(i, []);

            // Create a pool of valid candidate nodes (all nodes except self)
            List<int> candidates = [];
            for (int j = 0; j < numLocations; j++) {
                if (j != i) {
                    candidates.Add(j);
                }
            }

            // Randomly decide how many connections to add (1 to 4)
            int connectionCount = random.Next(numConnectionsRange.Item1, numConnectionsRange.Item2 + 1);

            // Randomly pick nodes from the candidates
            for (int k = 0; k < connectionCount; k++) {
                int randomIndex = random.Next(candidates.Count);
                _ = map[i].Add(candidates[randomIndex]);
                candidates.RemoveAt(randomIndex);
            }
        }

        return map;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    public static void QuitGame() {
        MainInstance.GetTree().Quit();
    }

    private void OnTerminalSubmit(InputEvent @event) {
        if (@event is InputEventKey keyEvent && keyEvent is { Pressed: true, Keycode: Key.Enter }) {
            this.Terminal.Execute(this.TerminalLineEdit.Text.Trim());
            this.TerminalLineEdit.Clear();
        }
    }
}
