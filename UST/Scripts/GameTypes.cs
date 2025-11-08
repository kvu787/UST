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
    public abstract float BaseHealth { get; set; }
    public abstract float BaseDefense { get; set; }
    public abstract float BaseAttackDamage { get; set; }
    public abstract float BaseAttackSpeed { get; set; }
    public abstract float BaseAccuracy { get; set; }

    public string Name { get; set; }
    public uint Team { get; set; }
    public float Health { get; set; }
}

public abstract class Unit : WorldObject {
    public abstract float BaseEvasion { get; set; }
    public abstract float BaseMovementSpeed { get; set; }
}

public abstract class Building : WorldObject;

public class Bastion : Unit {
    public override float BaseHealth { get; set; } = 1000;
    public override float BaseDefense { get; set; }
    public override float BaseAttackDamage { get; set; }
    public override float BaseAttackSpeed { get; set; }
    public override float BaseAccuracy { get; set; }
    public override float BaseEvasion { get; set; }
    public override float BaseMovementSpeed { get; set; }
}

public class Bunker : Building {
    public override float BaseHealth { get; set; } = 10000;
    public override float BaseDefense { get; set; }
    public override float BaseAttackDamage { get; set; }
    public override float BaseAttackSpeed { get; set; }
    public override float BaseAccuracy { get; set; }
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
