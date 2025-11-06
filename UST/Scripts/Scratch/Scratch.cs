using Godot;

namespace UST.Scratch;

public partial class Main : Node {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }
}

public enum UnitType {
    Rogue,
    Trader,
    Knight,
    Mage,
    Blacksmith
}

public enum BuildingType {
    Tavern,
    Arcanum,
    Barracks,
    Blacksmith,
    Castle,
    House,
    Market
}

public enum ItemType {
    LexingtonHandgun,
    ConcordBlade,
}

public enum ResourceType {
    Gold,
    Stone,
    Brick,
    Water,
    Wood,
    Food,
    Oil,
    Minerals
}

public enum LocationType {
    Swamp,
    Mountains,
    Plateau,
    Desert,
    Temperate,
    Wasteland,
    Tropics
}

public enum PathType {
    GravelPavement,
    DirtPavement,
    BrickPavement,
    StoneSlabPavement,
    AsphaltPavement,
    CobblestonePavement,
    Swamp,
    Mountain,
    Sand,
    Water,
    Forest,
    Ice,
    Snow,
    Plains,
    Mesa
}
