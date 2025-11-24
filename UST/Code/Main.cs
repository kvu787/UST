using Godot;

namespace UST;

public partial class Main : Node {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        GD.Print("Ready");
        GD.Print($"Game data folder: {ProjectSettings.GlobalizePath("user://")}");

        Globals.Terminal = new Terminal(this, this.GetNode<LineEdit>("LineEdit"));

        /*
        Generate the graph
        Generate mapping of graph vertices to team ownership (1 or 2)
        Generate units
        Place units on map
        */
        Globals.SaveData = new SaveData(numLocations: 10, probabilityAddExtraEdge: 0.2, proportionTeamOne: 0.65);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        /*
        Loop through and execute each unit's automaton
        if win condition is met, exit loop and print results
        */
        Globals.SaveData.Process(delta);
    }
}
