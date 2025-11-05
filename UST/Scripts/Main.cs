using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace UST.Scripts;

public partial class Main : Node {
    private LineEdit CommandInput;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        // Get reference to the LineEdit node
        this.CommandInput = this.GetNode<LineEdit>("LineEdit");

        // Connect to its GuiInput signal
        this.CommandInput.GuiInput += this.OnCommandInput;

        this.CommandInput.FocusEntered += () => this.CommandInput.PlaceholderText = "";
        this.CommandInput.FocusExited += () => this.CommandInput.PlaceholderText = "Click here. Type a command. Press ENTER to submit.";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    private void OnCommandInput(InputEvent @event) {
        if (@event is InputEventKey keyEvent && keyEvent is { Pressed: true, Keycode: Key.Enter }) {
            string text = this.CommandInput.Text.Trim();
            this.CommandInput.Clear();

            // Test: tater 'tater' 'ta te r' -tater '' --tater
            List<string> arguments = Regex.Matches(text, @"('[^']*')|([^' ]+)")
                .Select(x => x.Value)
                .Select(x => x.StartsWith("'", StringComparison.Ordinal) ? x[1..^1] : x)
                .ToList();

            foreach (string argument in arguments) {
                GD.Print(argument);
            }

            switch (text) {
                case "Exit":
                    this.GetTree().Quit();
                    break;
                case "CreateLocation":
                    break;
                case "ConnectAllNodes":
                    break;
                case "Backup":
                    break;
                default:
                    GD.Print("Unknown command!");
                    break;
            }

            if (text == "exit") {
                this.GetTree().Quit();
            }
        }
    }

    /*
    private static string HandleCommand(string command) {
        return new RootCommand("My graph management CLI") {
            new Command("add", "Add elements to the graph") {
                new Command("node", "Add a node to the graph") {
                    new Option<string>("--id", "The unique identifier for the node") { IsRequired = true },
                    new Option<string>("--label", "The label for the node")
                }.WithHandler((string id, string label) => {
                    Console.WriteLine($"Adding node: ID={id}, Label={label ?? "none"}");
                }),

                new Command("edge", "Add an edge between nodes") {
                    new Option<string>("--from", "Source node ID") { IsRequired = true },
                    new Option<string>("--to", "Target node ID") { IsRequired = true },
                    new Option<double>("--weight", () => 1.0, "Edge weight")
                }.WithHandler((string from, string to, double weight) => {
                    Console.WriteLine($"Adding edge: From={from}, To={to}, Weight={weight}");
                }),

                new Command("property", "Add a property to an element") {
                    new Option<string>("--element-id") { IsRequired = true },
                    new Option<string>("--key") { IsRequired = true },
                    new Option<string>("--value") { IsRequired = true }
                }.WithHandler((string elementId, string key, string value) => {
                    Console.WriteLine($"Adding property to {elementId}: {key}={value}");
                })
            }
        }.Invoke(args);
    }
    */
}

public enum UnitType {
}

public enum BuildingType {
}

public enum LocationType {
}

public enum ResourceType {
}

public enum TerrainType {
}

public enum PathType {
}

public class Unit {
    public string Name { get; set; }
    public UnitType Type { get; set; }
    public float Health { get; set; }
    public float Defense { get; set; }
    public float AttackDamage { get; set; }
    public float AttackSpeed { get; set; }
    public float Accuracy { get; set; }
    public float Evasion { get; set; }
    public float MovementSpeed { get; set; }
}

public class Building {
    public string Name { get; set; }
    public BuildingType Type { get; set; }
    public float Health { get; set; }
    public float Defense { get; set; }
    public float AttackDamage { get; set; }
    public float AttackSpeed { get; set; }
    public float Accuracy { get; set; }
}

public class Location {
    public string Name { get; set; }
    public LocationType Type { get; set; }
    public Collection<Unit> Units { get; }
    public Collection<Building> Buildings { get; }
    public Collection<Resource> Resources { get; }
}

public class Resource {
    public string Name { get; set; }
    public ResourceType Type { get; set; }
    public float Amount { get; set; }
}

public class Path {
    public string Name { get; set; }
    public PathType Type { get; set; }
    public Location Location1 { get; set; }
    public Location Location2 { get; set; }
    public float Distance { get; set; }
}
