using System;
using System.Collections.Generic;
using System.Linq;

namespace UST;

public class World {
    private readonly int LocationsCount;

    /// <summary>
    /// Maps each location to its neighbors.
    /// Guaranteed to be an undirected graph
    /// No self edges
    /// No duplicate edges
    /// Location IDs are from 0 to n
    /// There is at least 1 location
    /// </summary>
    public Dictionary<int, HashSet<int>> Map { get; }

    /// <summary>
    /// Maps each location to the team that owns that location.
    /// Index = location ID
    /// Value = the team that owns the location
    /// </summary>
    private readonly List<int> Teams;

    /// <summary>
    /// List of units.
    /// The index has no significance.
    /// The ordering has no significance.
    /// </summary>
    public List<Unit> Units { get; }

    /// <summary>
    /// Maps each location to the set of units in that location.
    /// </summary>
    private readonly Dictionary<int, HashSet<Unit>> UnitLocations;

    // NOTE: This will probably change to per-edge distances.
    private const double EdgeDistance = 1;

    public World() {
        this.LocationsCount = 3;
        double probabilityAddExtraEdge = 1;
        double proportionTeamOne = 0.5;
        int unitsCount = 20;

        this.Map = Graph.GenerateMap(this.LocationsCount, probabilityAddExtraEdge);
        //this.Teams = Graph.GenerateTeamMap(this.Map, proportionTeamOne);
        this.Units = GenerateUnits(unitsCount, this.LocationsCount);
        this.UnitLocations = [];
        for (int i = 0; i < this.Map.Count; i++) {
            this.UnitLocations[i] = [];
        }
        this.UpdateUnitLocations();
    }

    private void UpdateUnitLocations() {
        foreach (HashSet<Unit> units in this.UnitLocations.Values) {
            units.Clear();
        }
        foreach (Unit unit in this.Units) {
            _ = this.UnitLocations[unit.Location].Add(unit);
        }
    }

    private static List<Unit> GenerateUnits(int unitsCount, int locationsCount) {
        List<Unit> units = [];
        for (int i = 0; i < unitsCount; i++) {
            units.Add(new Unit {
                Id = i,
                Health = Random.Shared.NextDouble(80, 120),
                Attack = Random.Shared.NextDouble(0.5, 1.5),
                MoveSpeed = Random.Shared.NextDouble(0.8, 1.2),
                Team = Random.Shared.Next(2),
                Location = Random.Shared.Next(locationsCount),
                PreviousLocation = -1,
                TargetLocation = -1,
                MoveProgress = -1,
            });
        }
        return units;
    }

    public void Process(double delta) {
        // Move units
        foreach (Unit unit in this.Units) {
            if (unit.PreviousLocation == -1) {
                unit.MoveProgress = 0;
                unit.PreviousLocation = unit.Location;
                unit.TargetLocation = this.Map[unit.Location].ToList().PickRandom();
            }

            double remainingMoveDistance = delta * unit.MoveSpeed;
            while (remainingMoveDistance > 0) {
                double distanceToTarget = EdgeDistance - unit.MoveProgress;
                if (remainingMoveDistance >= distanceToTarget) {
                    unit.MoveProgress = 0;
                    unit.PreviousLocation = unit.Location;
                    unit.Location = unit.TargetLocation;
                    List<int> candidateNeighbors = this.Map[unit.Location].Where(x => x != unit.PreviousLocation).ToList();
                    if (candidateNeighbors.Count == 0) {
                        unit.TargetLocation = unit.PreviousLocation;
                    } else {
                        unit.TargetLocation = candidateNeighbors.PickRandom();
                    }
                    remainingMoveDistance -= distanceToTarget;
                } else {
                    unit.MoveProgress += remainingMoveDistance;
                    remainingMoveDistance = 0;
                }
            }
        }

        this.UpdateUnitLocations();

        // Attack units
        // Any opposing units in the same location will attack each other
        foreach ((_, HashSet<Unit> units) in this.UnitLocations) {
            List<IGrouping<int, Unit>> teamGroups = units.GroupBy(x => x.Team).ToList();
            if (teamGroups.Count < 2) {
                continue;
            }

            // For each team:
            // Accumulate total damage
            // Distribute among units from different teams
            foreach (IGrouping<int, Unit> ourTeamGroup in teamGroups) {
                int ourTeam = ourTeamGroup.Key;
                List<Unit> ourUnits = ourTeamGroup.ToList();
                double totalDamage = ourUnits.Sum(x => x.Attack * delta);
                List<Unit> enemyUnits = teamGroups.Where(x => x.Key != ourTeam).Select(x => x.ToList()).SelectMany(x => x).ToList();
                List<double> damageSplits = totalDamage.Split(enemyUnits.Count);
                for (int i = 0; i < enemyUnits.Count; i++) {
                    enemyUnits[i].Health -= damageSplits[i];
                }
            }
        }
    }

    private void CaptureLocations() {
        // Capture locations that contain units from one team
        foreach ((int location, HashSet<Unit> units) in this.UnitLocations) {
            if (units.GroupBy(x => x.Team).Count() == 1) {
                this.Teams[location] = units.First().Team;
            }
        }
    }

    private bool CheckVictory() {
        // Check if all locations captured by one team
        return this.Teams.GroupBy(x => x).Count() == 1;
    }
}
