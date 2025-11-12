using System;
using UST.Objects.Interfaces;

namespace UST.Objects.NaturalFormations;

public class Canyon : INaturalFormation {
    public Guid Id { get; set; }
    public string Name { get; set; }
}
