namespace UST.GameTypes;

public abstract class Resource {
    public required string Name { get; set; }
    public required float Amount { get; set; }
}

public class Gold : Resource {
}

public enum LocationType {
}

public enum ResourceType {
}

public enum TerrainType {
}

public enum PathType {
}

public abstract class WorldObject {
    public string Name { get; set; }
    public abstract float Health { get; set; }
    public abstract float Defense { get; set; }
    public abstract float AttackDamage { get; set; }
    public abstract float AttackSpeed { get; set; }
    public abstract float Accuracy { get; set; }
}

public abstract class Unit : WorldObject {
    public abstract float MovementSpeed { get; set; }
    public abstract float Evasion { get; set; }
}

public abstract class Building : WorldObject;

public class Bastion : Unit {
    public override float Health { get; set; } = 1000;
    public override float Defense { get; set; }
    public override float AttackDamage { get; set; }
    public override float AttackSpeed { get; set; }
    public override float Accuracy { get; set; }
    public override float MovementSpeed { get; set; }
    public override float Evasion { get; set; }
}

public class Bunker : Building {
    public override float Health { get; set; } = 10000;
    public override float Defense { get; set; }
    public override float AttackDamage { get; set; }
    public override float AttackSpeed { get; set; }
    public override float Accuracy { get; set; }
}

public class Location {
    public string Name { get; set; }
    public LocationType Type { get; set; }
}

public class Path {
    public PathType Type { get; set; }
    public Location Location1 { get; set; }
    public Location Location2 { get; set; }
    public float Distance { get; set; }
}
