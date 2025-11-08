using Godot;
using UST.GameTypes;

namespace UST;

public partial class Main : Node {
    private static Main MainInstance;
    private LineEdit TerminalLineEdit;
    private Terminal Terminal;

    public static bool SaveFileConnected { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        Unit bastion = new Bastion();
        bastion.Health -= 1;

        /*
         * Max health
         * Current health
         * Base attack
         *
         * Healing can't increase current health greater than max health
         */

        GD.Print("READY");
        GD.Print(ProjectSettings.GlobalizePath("user://"));

        MainInstance = this;
        this.Terminal = new();
        Gold gold = new() { Name = "g1", Amount = 2000, };

        this.TerminalLineEdit = this.GetNode<LineEdit>("LineEdit");
        this.TerminalLineEdit.GuiInput += this.OnTerminalSubmit;

        this.TerminalLineEdit.FocusEntered += () => this.TerminalLineEdit.PlaceholderText = "";
        this.TerminalLineEdit.FocusExited += () => this.TerminalLineEdit.PlaceholderText = "Click here. Type a command. Press ENTER to submit.";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    public static void QuitGame() {
        MainInstance.GetTree().Quit();
    }

    private void OnTerminalSubmit(InputEvent @event) {
        if (@event is InputEventKey keyEvent && keyEvent is { Pressed: true, Keycode: Key.Enter, }) {
            this.Terminal.Execute(this.TerminalLineEdit.Text.Trim());
            this.TerminalLineEdit.Clear();
        }
    }
}
