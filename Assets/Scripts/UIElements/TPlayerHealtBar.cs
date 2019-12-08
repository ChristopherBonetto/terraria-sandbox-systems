using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TPlayerHealtBar : MonoBehaviour
{
    [SerializeField] private TPlayerHealthContainer m_CurrentHeartsContainer;
    [SerializeField] private TPlayerHealthContainer m_MaxHeartsContainer;
    private TPlayerDefenseComponent m_TargetHealth;
    private int m_CurrentHeartsActive;
    private int m_MaxHeartsActive;

    /// <summary>
    /// The health component that it's referenced to.
    /// </summary>
    public TPlayerDefenseComponent TargetHealth => m_TargetHealth;

    /// <summary>
    /// Current health (from "model")
    /// </summary>
    public int Current => (int)m_TargetHealth.CurrentHealth;

    /// <summary>
    /// Max health (from "model")
    /// </summary>
    public int Max => (int)m_TargetHealth.MaxHealth;

    /// <summary>
    /// Current health (from "view")
    /// </summary>
    public int CurrentHeartsActive
    {
        get
        {
            m_CurrentHeartsActive = 0;

            foreach (var child in m_CurrentHeartsContainer.Hearts)
            {
                m_CurrentHeartsActive++;
            }

            return m_CurrentHeartsActive;
        }
    }

    /// <summary>
    /// Max health (from "view")
    /// </summary>
    private int MaxHeartsActive
    {
        get
        {
            m_MaxHeartsActive = 0;

            foreach (var child in m_MaxHeartsContainer.Hearts)
            {
                m_MaxHeartsActive++;
            }

            return m_MaxHeartsActive;
        }
    }


    private void OnEnable()
    {
        TEventManager.SubscribeTo<IDefend>(TEventID.OnHealthUpdate, UpdateHealthBar);
        TEventManager.SubscribeTo<TPlayerController>(TEventID.OnPlayerSpawned, SetTargetHealth);

        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemEquipped, OnItemEquipped);
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemUnequipped, OnItemUnequipped);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<IDefend>(TEventID.OnHealthUpdate, UpdateHealthBar);
        TEventManager.UnsubscribeFrom<TPlayerController>(TEventID.OnPlayerSpawned, SetTargetHealth);

        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemEquipped, OnItemEquipped);
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemUnequipped, OnItemUnequipped);
    }


    /// <summary>
    /// Update current health bar when some damage is taken.
    /// </summary>
    public virtual void UpdateHealthBar(IDefend defendComponent)
    {
        if (defendComponent == (IDefend)TargetHealth)
        {
            int length = m_CurrentHeartsContainer.Hearts.Count;

            // In case it takes more than one damage.
            for (int i = 0; i < defendComponent.MaxHealth; i++)
            {
                bool isActive = i < defendComponent.CurrentHealth;
                m_CurrentHeartsContainer.Hearts[i].enabled = isActive;
            }
        }
    }

    #region Equip Event Methods

    /// <summary>
    /// Set stats when an item is equipped
    /// </summary>
    public void OnItemEquipped(TInventorySlot item)
    {
        if (item.ItemInSlot.Item is TItemArmor)
        {
            TItemArmor armor = item.ItemInSlot.Item as TItemArmor;

            for (int i = 0; i < armor.Statistics.MaxHealth; i++)
            {
                // Can be optimized
                Image backHeart = ObjectPooler.SharedInstance.GetPooledObject("BackHeart").GetComponent<Image>();
                Image fullHeart = ObjectPooler.SharedInstance.GetPooledObject("FullHeart").GetComponent<Image>();

                backHeart.transform.SetParent(m_MaxHeartsContainer.transform);
                fullHeart.transform.SetParent(m_CurrentHeartsContainer.transform);

                backHeart.gameObject.SetActive(true);
                fullHeart.gameObject.SetActive(true);

                backHeart.enabled = true;
                fullHeart.enabled = false;

                if (MaxHeartsActive < Max + armor.Statistics.MaxHealth)
                {
                    m_MaxHeartsContainer.Hearts.Add(backHeart);
                    m_CurrentHeartsContainer.Hearts.Add(fullHeart);
                }
            }
        }
    }

    /// <summary>
    /// Set stats when an item is unequipped
    /// </summary>
    public void OnItemUnequipped(TInventorySlot item)
    {
        if (!item) return;

        // Update stats
        // Update sprites.

        if (item.ItemInSlot.Item is TItemArmor)
        {
            TItemArmor armor = item.ItemInSlot.Item as TItemArmor;

            for (int i = 0; i < armor.Statistics.MaxHealth; i++)
            {
                int index = MaxHeartsActive - 1 - i;

                m_MaxHeartsContainer.Hearts[index].gameObject.SetActive(false);
                m_CurrentHeartsContainer.Hearts[index].gameObject.SetActive(false);
            }
        }
    }

    #endregion

    private void SetTargetHealth(TPlayerController inPlayer)
    {
        m_TargetHealth = inPlayer.DefenseComponent as TPlayerDefenseComponent;
        InitHearts(inPlayer);
    }

    private void InitHearts(TPlayerController inPlayer)
    {
        for (int i = 0; i < inPlayer.DataAssigned.Statistics.MaxHealth; i++)
        {
            Image fullHeart = ObjectPooler.SharedInstance.GetPooledObject("FullHeart").GetComponent<Image>();
            Image backHeart = ObjectPooler.SharedInstance.GetPooledObject("BackHeart").GetComponent<Image>();

            fullHeart.transform.SetParent(m_CurrentHeartsContainer.transform);
            backHeart.transform.SetParent(m_MaxHeartsContainer.transform);

            m_CurrentHeartsContainer.Hearts.Add(fullHeart);
            m_MaxHeartsContainer.Hearts.Add(backHeart);

            fullHeart.gameObject.SetActive(true);
            backHeart.gameObject.SetActive(true);
        }
    }
}
