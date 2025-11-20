using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
            // Add the location
            HashSet<int> connectedLocations = [];
            map.Add(i, connectedLocations);

            // Create a pool of valid candidate nodes (all nodes except self)
            List<int> candidates = Enumerable.Range(0, numLocations).Where(j => j != i).ToList();

            // Randomly decide how many connections to add
            int connectionCount = random.Next(numConnectionsRange.Item1, numConnectionsRange.Item2 + 1);

            // Randomly pick nodes from the candidates
            for (int k = 0; k < connectionCount; k++) {
                int randomIndex = random.Next(candidates.Count);
                _ = connectedLocations.Add(candidates[randomIndex]);
                candidates.RemoveAt(randomIndex);
            }
        }

        return map;
    }

    /// <summary>
    /// Returns a graph (vertices and edges)
    /// The graph is undirected
    /// Graph contains no islands
    /// The edges from a vertex to other vertices are randomly chosen
    /// Vertices are not allowed to have edges to self
    /// Vertices are not allowed to have more than one edge to another vertex
    /// </summary>
    /// <param name="numLocations">the number of vertices</param>
    /// <param name="probabilityAddEdge"></param>
    /// <returns>a graph</returns>
    private static Dictionary<int, HashSet<int>> GenerateMap(int numLocations, float probabilityAddEdge) {
        if (numLocations < 1) {
            throw new ArgumentException("Must have at least 1 location");
        }
        if (probabilityAddEdge is < 0 or > 1) {
            throw new ArgumentOutOfRangeException(nameof(probabilityAddEdge), "Must be between 0 and 1");
        }

        Random random = new();
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

    /// <summary>
    /// Returns a graph (vertices and edges)
    /// The graph is undirected
    /// Each vertex has anywhere from numConnectionsRange.Item1 to numConnectionsRange.Item2 (inclusive) edges. This is randomly chosen for each vertex
    /// For example, if you pass in numConnectionsRange=(2, 5), then a vertex has at least 2 edges and at most 5 edges
    /// The edges from a vertex to other vertices are randomly chosen
    /// Vertices are not allowed to have edges to self
    /// Vertices are not allowed to have more than one edge to another vertex
    /// </summary>
    /// <param name="numLocations">the number of vertices</param>
    /// <param name="numConnectionsRange"></param>
    /// <returns>a graph</returns>
    private static Dictionary<int, HashSet<int>> GenerateMap2(int numLocations, (int, int) numConnectionsRange) {
        return null;
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
