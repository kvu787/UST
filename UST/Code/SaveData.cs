using System.Collections.Generic;

namespace UST;

public class SaveData {
    public Dictionary<int, HashSet<int>> Map { get; set; }
    public List<int> Teams { get; set; }
    public List<UnitRow> UnitRows { get; set; }
    public Dictionary<int, HashSet<UnitRow>> UnitLocations { get; set; }
}
