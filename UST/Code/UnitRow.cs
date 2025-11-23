namespace UST;

public class UnitRow {
    public double Health { get; set; }
    public int Location { get; set; }

    public double Attack { get; }
    public double MovementSpeed { get; }
    public int Team { get; }

    public bool Alive { get; set; }

    public UnitAutomaton UnitAutomaton { get; set; }
}
