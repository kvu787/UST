namespace UST.Objects.Interfaces;

public interface IBuilding : IEntity, ITeamMember {
    static abstract float MaxHealth { get; }
    static abstract float Defense { get; }

    static abstract float Accuracy { get; }
    static abstract float AttackDamage { get; }
    static abstract float AttackSpeed { get; }
}
