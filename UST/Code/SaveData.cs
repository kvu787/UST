using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UST;

public class SaveData {
    /// <summary>
    /// Map of locations to neighbors
    /// Guaranteed to be an undirected graph
    /// No self edges
    /// No duplicate edges
    /// Location IDs are from 0 to n
    /// There is at least 1 location
    /// </summary>
    private Dictionary<int, HashSet<int>> Map;

    /// <summary>
    /// Map of locations to what team owns them
    /// Index = location ID
    /// Value = the team that owns the location
    /// </summary>
    private List<int> Teams;

    private List<Unit> UnitRows;

    /// <summary>
    /// Maps each location to the set of units in that location.
    /// </summary>
    public Dictionary<int, HashSet<Unit>> UnitLocations { get; }

    public SaveData(int numLocations, double probabilityAddExtraEdge, double proportionTeamOne) {
        this.Map = Graph.GenerateMap(numLocations, probabilityAddExtraEdge);
        this.Teams = Graph.GenerateTeamMap(this.Map, proportionTeamOne);
    }

    public void Process(double delta) {
        // Capture locations that contain units from one team
        foreach ((int location, HashSet<Unit> units) in this.UnitLocations) {
            if (units.GroupBy(x => x.Team).Count() == 1) {
                this.Teams[location] = units.First().Team;
            }
        }

        // Check if all locations captured by one team
        if (this.Teams.GroupBy(x => x).Count() == 1) {
            GD.Print($"Team {this.Teams[0]} Victory!");
        }

        // Move units
        foreach (Unit unit in this.UnitRows) {
            // States:
            // Initial state
            // Moving
            // Finished movement
            if (unit.State == UnitState.Start) {
                List<int> neighbors = this.Map[unit.Location].ToList();
                if (unit.PreviousLocation == -1) {
                    unit.TargetLocation = neighbors.GetRandom();
                } else {
                    if (neighbors.Count == 1 && neighbors[0] == unit.PreviousLocation) {
                        unit.TargetLocation = unit.PreviousLocation;
                    } else {
                        _ = neighbors.Remove(unit.PreviousLocation);
                        unit.TargetLocation = neighbors.GetRandom();
                    }
                }

                unit.PreviousLocation = unit.Location;
            } else if (unit.State == UnitState.Moving) {
                double moveDistance = unit.MovementSpeed * delta;
                if (moveDistance >= (1 - unit.MoveProgress)) {
                    unit.Location = unit.TargetLocation;
                    unit.State = UnitState.Arrived;
                } else {
                    unit.MoveProgress += moveDistance;
                }
            } else if (unit.State == UnitState.Arrived) {
                // decide next location
            } else {
                throw new InvalidOperationException($"Unrecognized state: {unit.State}");
            }
        }

        // Attack units
    }
}
