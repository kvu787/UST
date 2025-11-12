using System;
using UST.Objects.Interfaces;

namespace UST.Spatial;

public class Location : IEntity {
    public Guid Id { get; set; }
    public string Name { get; set; }
}
