using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPlayerDefenseComponent : MonoBehaviour, IDefend
{
    [SerializeField] private float m_TimeToRegen;

    #region Private

    private float m_CurrentTimeRegen;
    private float m_MaxHealth;
    private float m_Defense;
    private float m_CurrentHealth;

    #endregion

    #region Property

    public float MaxHealth => m_MaxHealth;
    public float Defense => m_Defense;
    public float LastHealth => CurrentHealth;
    public float CurrentHealth
    {
        get { return m_CurrentHealth; }
        private set
        {
            m_CurrentHealth = Mathf.Clamp(value, 0, MaxHealth);

            TEventManager.TriggerEvent<IDefend>(TEventID.OnHealthUpdate, (IDefend)this);

            if (m_CurrentHealth <= 0)
                DisposeToDead();
        }
    }

    #endregion

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
        // Regeneration...

        HealthRegeneration();
    }

    #region initialization / Update Methods

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

    #endregion

    public void TakeDamage(float inAmount)
    {
        CurrentHealth -= Mathf.Max(1, inAmount - Defense);

        // After damage is taken invoke the event in the property.
    }

    private void HealthRegeneration()
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

    private void DisposeToDead()
    {
        gameObject.SetActive(false);
        TEventManager.TriggerEvent(TEventID.OnPlayerDied);
    }
}
