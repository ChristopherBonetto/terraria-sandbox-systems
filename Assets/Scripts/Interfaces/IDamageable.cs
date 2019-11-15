using UnityEngine;
using System.Collections;

public delegate void DamageEvent(float amount);

public interface IDamageable 
{
    /// <summary>
    /// Invoked this event when damage is dealt.
    /// </summary>
    event DamageEvent OnDamageEvent;


    /// <summary>
    /// Max health ( from model )
    /// </summary>
    float MaxHealth { get; }

    /// <summary>
    /// Current health. doesn't require the model. It's a resource.
    /// </summary>
    float CurrentHealth { get; }


    /// <summary>
    /// Initialize max health taken as the model one,
    /// Initialize current health = max health.
    /// </summary>
    void Init(float inMaxHealth);

    /// <summary>
    /// Reduce current health when it's called.
    /// </summary>
    void TakeDamage(float inAmount);

    /// <summary>
    /// Invoked OnDamage event.
    /// </summary>
    void OnDamageTaken(float inAmount);
}
