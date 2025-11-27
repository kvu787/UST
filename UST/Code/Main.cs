using System;
using Godot;

namespace UST;

public partial class Main : Node {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        if (!WindowsCommandPrompt.AllocConsole()) {
            throw new InvalidOperationException("AllocConsole() failed");
        }

        GD.Print("Ready");
        GD.Print($"Game data folder: {ProjectSettings.GlobalizePath("user://")}");

        Globals.Terminal = new Terminal(this, this.GetNode<LineEdit>("LineEdit"));
        Globals.World = new World();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        Globals.World.Process(delta);
    }
}
