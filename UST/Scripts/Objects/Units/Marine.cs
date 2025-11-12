using System;
using UST.Objects.Interfaces;

namespace UST.Objects.Units;

public class Marine : IUnit {
    public static float MaxHealth => 100;
    public static float Defense => 5;
    public static float Accuracy => 80;
    public static float AttackDamage => 10;
    public static float AttackSpeed => 3;
    public static float Evasion => 50;
    public static float MovementSpeed => 1;

    public float Health { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public uint Team { get; set; }
}
