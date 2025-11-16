using System;
using System.Collections.Generic;
using UST.Objects.Interfaces;
using UST.Spatial;

namespace UST;

enum UnitState {
    Idle,
    Move,
    Attack,
}

public class UnitAutomaton {
    private IUnit Unit;
    private UnitState State;
    private Location Location;

    private void ProcessFrame(TimeSpan deltaTime, Dictionary<Location, HashSet<Location>> map) {
        if (this.State == UnitState.Idle) {
            // Find the nearest unallied location
        } else if (this.State == UnitState.Move) {
            // If at goal location, then move to attack
            // Else: continue moving to goal location
        } else if (this.State == UnitState.Attack) {
            // If location is allied, then move to idle
            // Else: attack, uncapture, and capture
        }
    }
}

public static class MapUtils {
    public static void FindNearestUnalliedLocation(Location start, Dictionary<Location, HashSet<Location>> map) {
        ArgumentNullException.ThrowIfNull(start);

        if (start.Team == 0) {
            throw new InvalidOperationException("Invalid for neutral location");
        }

        // BFS here
    }
}
