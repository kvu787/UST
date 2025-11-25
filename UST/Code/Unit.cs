using System;
using System.Collections.Generic;

namespace UST;

public class Unit {
    public required double Health { get; set; }
    public required int Location { get; set; }

    /// <summary>
    /// Damage per second
    /// </summary>
    public required double Attack { get; set; }

    /// <summary>
    /// Meters per second
    /// </summary>
    public required double MovementSpeed { get; set; }
    public required int Team { get; set; }

    public required bool Alive { get; set; }

    public int PreviousLocation { get; set; }
    public int TargetLocation { get; set; }
    public double MoveProgress { get; set; }

    public UnitState State { get; set; }

    public static List<Unit> GenerateUnits(int count, int locationsCount) {
        List<Unit> units = [];
        for (int i = 0; i < count; i++) {
            units.Add(new Unit {
                Health = Random.Shared.NextDouble(90, 110),
                Attack = Random.Shared.NextDouble(4, 6),
                MovementSpeed = Random.Shared.NextDouble(0.8, 1.2),
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

public enum UnitState {
    Start,
    Moving,
    Arrived,
}
