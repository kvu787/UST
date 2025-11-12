using System;
using UST.Objects.Interfaces;

namespace UST.Spatial;

public class Path : IEntity {
    public Location Location1 { get; set; }
    public Location Location2 { get; set; }

    public Guid Id { get; set; }
    public string Name { get; set; }
}
