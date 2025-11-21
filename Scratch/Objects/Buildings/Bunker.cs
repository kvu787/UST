using UST.Objects.Interfaces;

namespace UST.Objects.Buildings;

public class Bunker : ITeamMember {
    public static readonly float MaxHealth = 1000;
    public static readonly float Defense;
    public static readonly float AttackDamage;
    public static readonly float AttackSpeed;
    public static readonly float Accuracy;

    public string Name { get; set; }
    public uint Team { get; set; }
    public float Health { get; set; }
}
