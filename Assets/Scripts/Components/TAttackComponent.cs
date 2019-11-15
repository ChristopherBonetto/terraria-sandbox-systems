using UnityEngine;
using System.Collections;

public class TAttackComponent : MonoBehaviour, IAttack
{
    public event AttackEvent OnAttackEvent;

    private float m_AttackDamage;
    public float AttackDamage { get { return m_AttackDamage; } }

    private float m_AttackDelay;
    public float AttackDelay { get { return m_AttackDelay; } }

    private float m_CurrentDelay;


    public void Init(float inAttackDamage)
    {
        m_AttackDamage = inAttackDamage;
    }

    public void Init(float inAttackDamage, float inAttackDelay)
    {
        m_AttackDamage = inAttackDamage;
        m_AttackDelay = inAttackDelay;
    }

    public void ExecuteDelay()
    {
        if (m_CurrentDelay > 0)
        {
            m_CurrentDelay -= Time.deltaTime;
        }
    }

    public void ExecuteAttack()
    {
        throw new System.NotImplementedException();
    }

    public void OnExecuteAttack()
    {
        OnAttackEvent?.Invoke(AttackDamage);
    }
}
