using System;
using System.Collections.Generic;
using System.Diagnostics;
using UST.Objects.Interfaces;
using UST.Spatial;

namespace UST;

enum UnitState {
    Idle,
    Move,
    Attack,
}

enum UnitState2 {
    DetermineTarget,
    Move,
    Attack,
}

public class UnitAutomaton {
    private IUnit Unit;
    private UnitState State;
    private UnitState2 State2;
    private Location Location;

    private Stopwatch Stopwatch;
    private List<Location> PathToTarget;
    private int CurrentLocationIndex;

    /*
ProcessFrame(deltaTime):
    while True:
        if this.State == DetermineTarget:
            if this.Target is null:
                this.Target = GetPathToNearestUnallied()
            if this.TimeRemaining >= deltaTime:
                this.TimeRemaining -= deltaTime
                return
            else:
                 deltaTime -= this.TimeRemaining
                 this.State = Move
        if this.State == Move:
            ...
        if this.State == Arrived:
            ...
        if this.State == Attacking:
            ...
     */

    private void ProcessFrame(TimeSpan deltaTime, Dictionary<Location, HashSet<Location>> map) {
        if (this.State2 == UnitState2.DetermineTarget) {
            if (this.Stopwatch is null) {
                // Target hasn't been computed yet
            } else {
                // Target has been computed and thinking stopwatch is running
                if (this.Stopwatch.Elapsed < TimeSpan.FromSeconds(1)) {
                    // Keep thinking...
                } else {
                    // Start moving to target
                    this.State2 = UnitState2.Move;
                    this.Stopwatch.Reset();
                }
            }
        }

        if (this.State2 == UnitState2.Move) {
            if (this.Stopwatch.Elapsed >= TimeSpan.FromSeconds(1)) {
                this.CurrentLocationIndex++;
                this.Stopwatch.Restart();
            }

            if (this.CurrentLocationIndex == (this.PathToTarget.Count - 1)) {
                this.State2 = UnitState2.Attack;
            }
        }

        if (this.State2 == UnitState2.Attack) {
            // if target unit is present, then attack
            // else: go back to state=DetermineTarget
        }

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
