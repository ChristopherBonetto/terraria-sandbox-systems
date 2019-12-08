/// <summary>
/// Extension of IDamageable interface
/// </summary>
public interface IDefend : IDamageable
{
    /// <summary>
    /// Entity's defense. ( from model )
    /// </summary>
    float Defense { get; }

    /// <summary>
    /// Initialize the max health and the armor as the model one.
    /// </summary>
    void Init(float inMaxHealth, float inDefense);

    /// <summary>
    /// Update max health in case the entity is buffed.
    /// </summary>
    void UpdateMaxHealth(float inMaxHealth);

    /// <summary>
    /// Update defense stats in case the entity is buffed.
    /// </summary>
    void UpdateDefenseStats(float inMaxHealth, float inDefense);
}