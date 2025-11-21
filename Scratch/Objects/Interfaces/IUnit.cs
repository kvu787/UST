namespace UST.Objects.Interfaces;

public interface IUnit : IEntity, ITeamMember {
    static abstract float MaxHealth { get; }
    static abstract float Defense { get; }

    static abstract float Accuracy { get; }
    static abstract float AttackDamage { get; }
    static abstract float AttackSpeed { get; }

    static abstract float Evasion { get; }
    static abstract float MovementSpeed { get; }
}
