using System.Collections.Generic;
using UST.GameTypes;

namespace UST;

public sealed class SaveFile {
    public List<Unit> Units { get; set; }
    public List<Building> Buildings { get; set; }
    public List<Location> Locations { get; set; }
    public List<Path> Paths { get; set; }
}
