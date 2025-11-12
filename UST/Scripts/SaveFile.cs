using System.Collections.Generic;
using UST.Spatial;

namespace UST;

public sealed class SaveFile {
    public List<object> ObjectsInSimWorld { get; set; }
    public List<Location> Locations { get; set; }
    public List<Path> Paths { get; set; }
}
