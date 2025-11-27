namespace UST;

public class Unit {
    public required double Health { get; set; }

    /// <summary>
    /// Health points per second
    /// </summary>
    public required double Attack { get; set; }

    /// <summary>
    /// Meters per second
    /// </summary>
    public required double MoveSpeed { get; set; }

    public required int Team { get; set; }

    public required int Location { get; set; }

    public required int PreviousLocation { get; set; }

    public required int TargetLocation { get; set; }

    /// <summary>
    /// How many meters the unit has traveled on the path from current to target location.
    /// </summary>
    public required double MoveProgress { get; set; }
}
