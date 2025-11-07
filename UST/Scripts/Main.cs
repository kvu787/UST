using Godot;

namespace UST;

public partial class Main : Node {
    private static Main MainInstance;
    private LineEdit CommandInput;
    private CommandProcessor CommandProcessor;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        MainInstance = this;
        this.CommandProcessor = new();
        Gold gold = new() { Name = "g1", Amount = 2000 };

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
        if (@event is InputEventKey keyEvent && keyEvent is { Pressed: true, Keycode: Key.Enter }) {
            string text = this.CommandInput.Text.Trim();
            this.CommandInput.Clear();
            this.CommandProcessor.Execute(text);
        }
    }
}
