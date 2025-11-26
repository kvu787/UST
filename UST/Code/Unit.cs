using System;
using System.Collections.Generic;

namespace UST;

public class Unit {
    public double Health { get; set; }

    /// <summary>
    /// Health points per second
    /// </summary>
    public double Attack { get; set; }

    /// <summary>
    /// Meters per second
    /// </summary>
    public int Team { get; set; }

    public bool Alive { get; set; }

    public double MoveSpeed { get; set; }
    public int Location { get; set; }
    public int PreviousLocation { get; set; }
    public int TargetLocation { get; set; }

    /// <summary>
    /// How much distance the unit has traveled on the path from current to target location.
    /// </summary>
    public double MoveProgress { get; set; }

    public static List<Unit> GenerateUnits(int count, int locationsCount) {
        List<Unit> units = [];
        for (int i = 0; i < count; i++) {
            units.Add(new Unit {
                Health = Random.Shared.NextDouble(90, 110),
                Attack = Random.Shared.NextDouble(4, 6),
                MoveSpeed = Random.Shared.NextDouble(0.8, 1.2),
                Location = Random.Shared.Next(locationsCount),
                Team = Random.Shared.Next(2),
                Alive = false,
                PreviousLocation = -1,
                TargetLocation = -1,
                MoveProgress = -1,
            });
        }
        return units;
    }
}
