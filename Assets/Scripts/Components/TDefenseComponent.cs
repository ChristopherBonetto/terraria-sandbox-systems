using UnityEngine;
using System.Collections;

public class TDefenseComponent : MonoBehaviour, IDefend
{ 
    private float m_MaxHealth;
    public float MaxHealth => m_MaxHealth;

    private float m_Defense;
    public float Defense => m_Defense;

    private float m_CurrentHealth;
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
    /// Editor testing.
    /// </summary>
    [ContextMenu("Take Damage")]
    public void DamageEntity()
    {
        TakeDamage(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.RightControl))
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
        CurrentHealth -= Mathf.Max(1, inAmount - Defense);
    }

    private void DisposeToDead()
    {
        gameObject.SetActive(false);
    }
}
