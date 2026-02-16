using Godot;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Environment = System.Environment;

namespace UST;

/// <summary>
/// Olivia: Walks around, invulnerable
/// Liam: Walks around, dies after set amount of time
/// Emma: Walks around with variable speed, dies after variable amount of time
/// Noah: Seeks out food to eat to increase life time
/// Amelia: Can fish from lakes
/// Oliver: Requires shelter
/// Charlotte: Requires water
/// 
/// </summary>

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
        GD.Print();

        this.TextOutput = this.GetNode<TextEdit>("TextEdit");

        PrintSystemInfo();

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

    private static void PrintSystemInfo() {
        GD.Print("=== Runtime Info ===");
        GD.Print($"RuntimeInformation.FrameworkDescription: {RuntimeInformation.FrameworkDescription}");
        GD.Print($"RuntimeInformation.OSDescription: {RuntimeInformation.OSDescription}");
        GD.Print($"RuntimeInformation.ProcessArchitecture: {RuntimeInformation.ProcessArchitecture}");
        GD.Print($"Environment.Version: {Environment.Version}");
        GD.Print($"AppContext.TargetFrameworkName: {AppContext.TargetFrameworkName}");
        GD.Print();

        GD.Print("=== Process Info ===");
        GD.Print($"Environment.ProcessId: {Environment.ProcessId}");
        GD.Print($"Environment.ProcessPath: {Environment.ProcessPath}");
        GD.Print($"Environment.CurrentDirectory: {Environment.CurrentDirectory}");
        GD.Print();

        GD.Print("=== Machine Info ===");
        GD.Print($"Environment.MachineName: {Environment.MachineName}");
        GD.Print($"Environment.ProcessorCount: {Environment.ProcessorCount}");
        GD.Print($"Environment.Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");
        GD.Print();

        GD.Print("=== Memory ===");
        GD.Print($"GC.GetTotalMemory: {GC.GetTotalMemory(false)}");
        GD.Print();
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
