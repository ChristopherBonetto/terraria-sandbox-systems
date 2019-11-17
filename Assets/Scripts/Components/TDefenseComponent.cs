using UnityEngine;
using System.Collections;

public class TDefenseComponent : MonoBehaviour, IDefend
{
    public event DamageEvent OnDamageEvent;

    private float m_MaxHealth;
    public float MaxHealth => m_MaxHealth;

    private float m_CurrentHealth;
    public float CurrentHealth => m_CurrentHealth;

    private float m_Defense;
    public float Defense => m_Defense;


    /// <summary>
    /// Editor testing.
    /// </summary>
    [ContextMenu("Take Damage")]
    public void DamageEntity()
    {
        OnDamageTaken(1);
    }

    private void OnEnable()
    {
        OnDamageEvent += TakeDamage;
    }

    private void OnDisable()
    {
        OnDamageEvent -= TakeDamage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            DamageEntity();
    }

    public void Init(float inMaxHealth)
    {
        m_MaxHealth = inMaxHealth;
        m_CurrentHealth = m_MaxHealth;
    }

    public void Init(float inMaxHealth, float inDefense)
    {
        m_MaxHealth = inMaxHealth;
        m_CurrentHealth = m_MaxHealth;
        m_Defense = inDefense;
    }

    public void TakeDamage(float inAmount)
    {
        m_CurrentHealth -= Mathf.Max(1, inAmount - Defense);

        // Call dead functions...
        if (m_CurrentHealth <= 0)
            gameObject.SetActive(false);
    }

    public void OnDamageTaken(float inAmount)
    {
        OnDamageEvent(inAmount);
    }
}
