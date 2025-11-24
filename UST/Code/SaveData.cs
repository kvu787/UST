using System.Collections.Generic;

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

    private List<UnitRow> UnitRows;
    public Dictionary<int, HashSet<UnitRow>> UnitLocations { get; }

    public SaveData(int numLocations, double probabilityAddExtraEdge, double proportionTeamOne) {
        this.Map = Graph.GenerateMap(numLocations, probabilityAddExtraEdge);
        this.Teams = Graph.GenerateTeamMap(this.Map, proportionTeamOne);
    }

    public void Process(double delta) {
    }
}
