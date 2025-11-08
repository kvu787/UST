using Godot;
using UST.GameTypes;

namespace UST;

public partial class Main : Node {
    private static Main MainInstance;
    private LineEdit CommandInput;
    private CommandProcessor CommandProcessor;

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
        this.CommandProcessor = new();
        Gold gold = new() { Name = "g1", Amount = 2000, };

        this.CommandInput = this.GetNode<LineEdit>("LineEdit");
        this.CommandInput.GuiInput += this.OnCommandInput;

        this.CommandInput.FocusEntered += () => this.CommandInput.PlaceholderText = "";
        this.CommandInput.FocusExited += () => this.CommandInput.PlaceholderText = "Click here. Type a command. Press ENTER to submit.";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    public static void QuitGame() {
        MainInstance.GetTree().Quit();
    }

    private void OnCommandInput(InputEvent @event) {
        if (@event is InputEventKey keyEvent && keyEvent is { Pressed: true, Keycode: Key.Enter, }) {
            this.CommandProcessor.Execute(this.CommandInput.Text.Trim());
            this.CommandInput.Clear();
        }
    }
}
