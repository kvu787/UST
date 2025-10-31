using Godot;
using System.Collections.ObjectModel;

namespace UST.Scripts.SciFi;

public partial class Main : Node {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }
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
