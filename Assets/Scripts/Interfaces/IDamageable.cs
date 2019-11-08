using UnityEngine;
using System.Collections;

public delegate void Damage(float amount);

public interface IDamageable 
{
    event Damage OnDamage;

    float MaxHealth { get; }
    float CurrentHealth { get; }

    void Init(float inMaxHealth);
    void TakeDamage(float inAmount);
    void OnDamageTaken(float inAmount);
}
