using System;
using UST.Objects.Interfaces;

namespace UST.Objects.Units;

public class QuarterTonRecon : IUnit {
    public static float MaxHealth => 500;
    public static float Defense => 20;
    public static float Accuracy => 70;
    public static float AttackDamage => 50;
    public static float AttackSpeed => 3;
    public static float Evasion => 50;
    public static float MovementSpeed => 4;

    public float Health { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public uint Team { get; set; }
}
