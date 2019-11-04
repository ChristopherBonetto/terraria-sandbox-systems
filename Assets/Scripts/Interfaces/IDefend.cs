public interface IDefend : IDamageable
{
    float Defense { get; }
    void Init(float inMaxHealth, float inDefense);
}