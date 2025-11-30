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
        this.TextOutput = this.GetNode<TextEdit>("TextEdit");


        Globals.Terminal = new Terminal(this, this.GetNode<LineEdit>("LineEdit"));
        Globals.World = new World();

        GD.Print();
        GD.Print("Generated map:");
        GD.Print(Graph.MapToString(Globals.World.Map));

        GD.Print();
        GD.Print("Generated units:");
        foreach (Unit unit in Globals.World.Units) {
            GD.Print($"Unit {unit.Id,2}: {unit.Health,6:###.00}, {unit.MoveSpeed:F2}, {unit.Attack:F2}, {unit.Location}, {unit.Team}");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        Globals.World.Process(delta);

        StringBuilder sb = new();
        foreach (Unit unit in Globals.World.Units) {
            _ = sb.AppendLine($"Unit {unit.Id,2}: PreviousLocation={unit.PreviousLocation}, Location={unit.Location}, TargetLocation={unit.TargetLocation}, MoveProgress={unit.MoveProgress:0.00}");
        }
        _ = sb.AppendLine();
        foreach (Unit unit in Globals.World.Units) {
            _ = sb.AppendLine($"Unit {unit.Id,2}: Health={unit.Health,6:###.00}");
        }
        this.TextOutput.Text = sb.ToString();
    }
}
