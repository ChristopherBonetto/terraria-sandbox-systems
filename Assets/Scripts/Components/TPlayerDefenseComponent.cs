using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPlayerDefenseComponent : MonoBehaviour, IDefend
{
    /// <summary>
    /// Property
    /// </summary>
    public float MaxHealth => m_MaxHealth;
    public float Defense => m_Defense;
    public float LastHealth => CurrentHealth;
    public float CurrentHealth
    {
        get { return m_CurrentHealth; }
        private set
        {
            var lastHealth = m_CurrentHealth;
            m_CurrentHealth = Mathf.Clamp(value, 0, MaxHealth);

            // Call UI event.
            TEventManager.TriggerEvent<IDefend>(TEventID.OnHealthUpdate, (IDefend)this);

            if (m_CurrentHealth <= 0)
                DisposeToDead();

        }
    }

    /// <summary>
    /// Private
    /// </summary>
    private float m_MaxHealth;
    private float m_Defense;
    private float m_CurrentHealth;

    [SerializeField] private float m_TimeToRegen;
    private float m_CurrentTimeRegen;


    /// <summary>
    /// Editor testing.
    /// </summary>
    [ContextMenu("Take Damage")]
    public void DamageEntity()
    {
        TakeDamage(1);
    }

    private void Update()
    {
        if (CurrentHealth < MaxHealth)
        {
            if (m_CurrentTimeRegen < 0)
            {
                CurrentHealth += 1;
                m_CurrentTimeRegen = m_TimeToRegen;
            }
            else
                m_CurrentTimeRegen -= Time.deltaTime;
        }
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

    public void UpdateMaxHealth(float inMaxHealth)
    {
        if (m_CurrentHealth > inMaxHealth)
            m_CurrentHealth = inMaxHealth;

        m_MaxHealth = inMaxHealth;
    }

    public void UpdateDefenseStats(float inMaxHealth, float inDefense)
    {
        if (m_CurrentHealth > inMaxHealth)
            m_CurrentHealth = inMaxHealth;

        m_MaxHealth = inMaxHealth;
        m_Defense = inDefense;
    }

    public void TakeDamage(float inAmount)
    {
        CurrentHealth -= Mathf.Max(1, inAmount - Defense);
    }

    private void DisposeToDead()
    {
        gameObject.SetActive(false);
        TEventManager.TriggerEvent(TEventID.OnPlayerDied);
    }

}
