using Godot;
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

        this.map = Graph.GenerateMap(25, 0.1);
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
