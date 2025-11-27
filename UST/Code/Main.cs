using Godot;
using System;
using System.Text;

namespace UST;

public partial class Main : Node {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "This object should last the entire process lifecycle")]
    private TextEdit TextOutput;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        if (!WindowsCommandPrompt.AllocConsole()) {
            throw new InvalidOperationException("AllocConsole() failed");
        }

        GD.Print("Ready");
        GD.Print($"Game data folder: {ProjectSettings.GlobalizePath("user://")}");

        Globals.Terminal = new Terminal(this, this.GetNode<LineEdit>("LineEdit"));
        Globals.World = new World();
        this.TextOutput = this.GetNode<TextEdit>("TextEdit");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        Globals.World.Process(delta);

        // Output each unit and location
        StringBuilder s = new();
        foreach (Unit unit in Globals.World.Units) {
            _ = s.AppendLine($"Unit {unit.Id}: PreviousLocation={unit.PreviousLocation}, Location={unit.Location}, TargetLocation={unit.TargetLocation}, MoveProgress={unit.MoveProgress:0.00}");
        }
        this.TextOutput.Text = s.ToString();
    }
}
