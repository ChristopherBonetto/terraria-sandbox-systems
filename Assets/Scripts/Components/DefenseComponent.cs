using UnityEngine;
using System.Collections;

public class DefenseComponent : MonoBehaviour, IDefend
{
    private float m_MaxHealth;
    private float m_CurrentHealth;
    private float m_Defense;

    public float MaxHealth => m_MaxHealth;
    public float CurrentHealth => m_CurrentHealth;
    public float Defense => m_Defense;


    protected virtual void OnEnable()
    {
        // Subscribe.
    }

    protected virtual void OnDisable()
    {
        // Unsuscribe.
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

    public void OndamageTaken(float inAmount)
    {
        // Call event
    }

    public void TakeDamage(float inAmount)
    {
        m_CurrentHealth -= Mathf.Max(1, inAmount - Defense);

        // Call dead functions...
        if (m_CurrentHealth <= 0)
            gameObject.SetActive(false);
    }
}
