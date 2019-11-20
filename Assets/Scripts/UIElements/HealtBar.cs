using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour
{
    [SerializeField]
    private TDefenseComponent m_TargetHealth;
    public TDefenseComponent Targethealth => m_TargetHealth;

    [SerializeField]
    private Slider m_Slider;


    private void OnEnable()
    {
        TEventManager.SubscribeTo<IDefend>(TEventID.OnHealthUpdate, UpdateHealthBar);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<IDefend>(TEventID.OnHealthUpdate, UpdateHealthBar);
    }

    /// <summary>
    /// Update health bar when some damage is taken.
    /// </summary>
    public void UpdateHealthBar(IDefend defendComponent)
    {
        if (defendComponent == (IDefend)Targethealth)
        {
            m_Slider.maxValue = defendComponent.MaxHealth;
            m_Slider.value = defendComponent.CurrentHealth;
        }
    }
}
