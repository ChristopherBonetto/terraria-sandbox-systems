using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AttackEvent(float inAttackDamage);

public interface IAttack
{
    /// <summary>
    /// Event invoked when an entity decide to attack.
    /// </summary>
    event AttackEvent OnAttackEvent;

    /// <summary>
    /// Item attack damage.
    /// </summary>
    float AttackDamage { get; }

    /// <summary>
    /// Item attack delay.
    /// </summary>
    float AttackDelay { get; }


    /// <summary>
    /// Initialize Attack damage.
    /// </summary>
    void Init(float inAttackDamage);

    /// <summary>
    /// Initialize Attack damage, attack delay.
    /// </summary>
    void Init(float inAttackDamage, float inAttackDelay);

    /// <summary>
    /// Execute attack operation.
    /// </summary>
    void ExecuteAttack(TItemWeapon inWeapon);

    /// <summary>
    /// Execute attack delay.
    /// </summary>
    void ExecuteDelay();

    /// <summary>
    /// Invoke OnAttakEvent.
    /// </summary>
    void OnExecuteAttack();
}
