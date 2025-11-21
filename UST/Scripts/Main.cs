using Godot;
using System;
using System.Collections.Generic;

namespace UST;

public interface IAutomaton {
    public void ProcessFrame(double delta, Unit unit);
}

public class Unit {

}

public partial class Main : Node {
    private static Main MainInstance;
    private LineEdit TerminalLineEdit;
    private Terminal Terminal;
    public static bool SaveFileConnected { get; set; }

    // Game data
    private Dictionary<int, HashSet<int>> map;

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

        this.map = GenerateMap(25, 0.1);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    /// <summary>
    /// Generates a randomly connected graph (vertices and edges)
    /// The graph is undirected
    /// Graph contains no islands
    /// The edges from a vertex to other vertices are randomly chosen
    /// Vertices are not allowed to have edges to self
    /// Vertices are not allowed to have more than one edge to another vertex
    /// </summary>
    /// <param name="numLocations"></param>
    /// <param name="probabilityAddEdge"></param>
    /// <returns></returns>
    private static Dictionary<int, HashSet<int>> GenerateMap(int numLocations, double probabilityAddEdge) {
        if (numLocations < 1) {
            throw new ArgumentOutOfRangeException(nameof(numLocations), "Must have at least 1 location");
        }

        if (probabilityAddEdge is < 0 or > 1) {
            throw new ArgumentOutOfRangeException(nameof(probabilityAddEdge), "Must be between 0 and 1");
        }

        Random random = Random.Shared;
        HashSet<(int, int)> edges = [];

        for (int i = 1; i < numLocations; i++) {
            int connectedLocation = random.Next(i);
            (int, int) newEdge = (connectedLocation, i);
            _ = edges.Add(newEdge);
        }

        for (int i = 0; i < numLocations; i++) {
            for (int j = i + 1; j < numLocations; j++) {
                if (random.NextDouble() < probabilityAddEdge) {
                    _ = edges.Add((i, j));
                }
            }
        }

        Dictionary<int, HashSet<int>> graph = [];
        for (int i = 0; i < numLocations; i++) {
            graph[i] = [];
        }

        foreach ((int from, int to) in edges) {
            _ = graph[from].Add(to);
            _ = graph[to].Add(from);
        }

        return graph;
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
