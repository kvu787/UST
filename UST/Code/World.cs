using Godot;
using System.Collections.Generic;
using System.Linq;

namespace UST;

public class World {
    /// <summary>
    /// Maps each location to its neighbors.
    /// Guaranteed to be an undirected graph
    /// No self edges
    /// No duplicate edges
    /// Location IDs are from 0 to n
    /// There is at least 1 location
    /// </summary>
    private Dictionary<int, HashSet<int>> Map;

    /// <summary>
    /// Maps each location to the team that owns that location.
    /// Index = location ID
    /// Value = the team that owns the location
    /// </summary>
    private List<int> Teams;

    /// <summary>
    /// List of units.
    /// The index has no significance.
    /// The ordering has no significance.
    /// </summary>
    private List<Unit> Units;

    /// <summary>
    /// Maps each location to the set of units in that location.
    /// </summary>
    private Dictionary<int, HashSet<Unit>> UnitLocations;

    // NOTE: This will probably change to per-edge distances.
    private const double EdgeDistance = 3;

    public World(int numLocations, double probabilityAddExtraEdge, double proportionTeamOne) {
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

        /*
         * When the unit is first initialized, find the move target
         * loop:
         *   Move until time runs out, or target is reached
         *   If time left and target is reached: update location, find next target, repeat loop
         *   If just target is reached: update location, find next target, exit
         *   If just time ran out: exit
         */

        // Move units
        foreach (Unit unit in this.Units) {
            if (unit.PreviousLocation == -1) {
                unit.MoveProgress = 0;
                unit.PreviousLocation = unit.Location;
                unit.TargetLocation = this.Map[unit.Location].ToList().GetRandom();
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
                        unit.TargetLocation = candidateNeighbors.GetRandom();
                    }
                    remainingMoveDistance -= distanceToTarget;
                } else {
                    unit.MoveProgress += remainingMoveDistance;
                    remainingMoveDistance = 0;
                }
            }
        }

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
}
