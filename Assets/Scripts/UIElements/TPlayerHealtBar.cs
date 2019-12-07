using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TPlayerHealtBar : MonoBehaviour
{
    [SerializeField]
    protected TPlayerDefenseComponent m_TargetHealth;
    public TPlayerDefenseComponent Targethealth => m_TargetHealth;

    [SerializeField]
    protected Slider m_Slider;


    private void OnEnable()
    {
        TEventManager.SubscribeTo<IDefend>(TEventID.OnHealthUpdate, UpdateHealthBar);
        TEventManager.SubscribeTo<TPlayerController>(TEventID.OnPlayerSpawned, SetTargetHealth);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<IDefend>(TEventID.OnHealthUpdate, UpdateHealthBar);
        TEventManager.UnsubscribeFrom<TPlayerController>(TEventID.OnPlayerSpawned, SetTargetHealth);
    }

    /// <summary>
    /// Update health bar when some damage is taken.
    /// </summary>
    public virtual void UpdateHealthBar(IDefend defendComponent)
    {
        if (defendComponent == (IDefend)Targethealth)
        {
            m_Slider.maxValue = defendComponent.MaxHealth;
            m_Slider.value = defendComponent.CurrentHealth;
        }
    }

    private void SetTargetHealth(TPlayerController inPlayer)
    {
        m_TargetHealth = inPlayer.DefenseComponent as TPlayerDefenseComponent;
    }
}
