using System;
using System.Runtime.InteropServices;
using Godot;

namespace UST;

public partial class Main : Node {
    [LibraryImport("kernel32.dll")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        if (!AllocConsole()) {
            throw new InvalidOperationException("AllocConsole() failed");
        }

        GD.Print("Ready");
        GD.Print($"Game data folder: {ProjectSettings.GlobalizePath("user://")}");

        Globals.Terminal = new Terminal(this, this.GetNode<LineEdit>("LineEdit"));
        Globals.World = new World(numLocations: 10, probabilityAddExtraEdge: 0.2, proportionTeamOne: 0.65);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        //Globals.SaveData.Process(delta);
    }
}
