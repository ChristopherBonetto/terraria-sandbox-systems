using UnityEngine;
using System.Collections;

public class TAttackComponent : MonoBehaviour, IWeapon
{
    public event AttackEvent OnAttackEvent;

    private float m_AttackDamage;
    public float AttackDamage { get { return m_AttackDamage; } }

    private float m_AttackDelay;
    public float AttackDelay { get { return m_AttackDelay; } }

    [SerializeField]
    private TItemWeapon m_WeaponToEquip;
    public TItemWeapon WeaponEquipped => m_WeaponToEquip;

    private float m_CurrentDelay;


    private void OnEnable()
    {
        OnAttackEvent += ExecuteAttack;
    }

    private void OnDisable()
    {
        
        OnAttackEvent -= ExecuteAttack;
    }

    private void Update()
    {
        ExecuteDelay();
    }

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

    public void ExecuteAttack(float inAmount)
    {
        if (m_CurrentDelay <= 0)
        {
            WeaponEquipped.OnUse();
        }
    }

    public void OnExecuteAttack()
    {
        OnAttackEvent?.Invoke(AttackDamage);
    }
}
