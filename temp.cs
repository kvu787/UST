using System.Collections.Generic;
using Godot;

namespace UST;

public partial class Main : Node {
    /*
    King of the hill:
    A---B
    |\ /|
    | E |
    |/ \|
    D---C
    */


    private readonly List<string> Nodes = ["A", "B", "C", "D", "E"];
    private readonly List<(string, string)> Edges = [
        ("A", "B"),
        ("B", "C"),
        ("C", "D"),
        ("D", "A"),
        ("E", "A"),
        ("E", "B"),
        ("E", "C"),
        ("E", "D"),
    ];


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }
}


public enum UnitType {
    Bastion,
    Klydac,
}

public class Unit {
    public UnitType UnitType;
    public float Health;
    public string Node;
}

public enum BastionStates {
    Idle,
    Move,
    Attack,
    Defend,
    Retreat,
    Rest,
}

public class Bastion {
    public float Health;
    public float Energy;
    public string Node;
    public string State;
    public float LockDuration;
}

