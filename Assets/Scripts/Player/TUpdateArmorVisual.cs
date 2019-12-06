using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUpdateArmorVisual : MonoBehaviour
{
    [SerializeField] private ArmorType m_Type;
    [SerializeField] private Sprite m_DefaultArmor;
    [SerializeField] private SpriteRenderer m_ArmorRenderer;

    private TItemArmor m_LastArmor;

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemEquipped, UpdateArmorRenderer);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemEquipped, UpdateArmorRenderer);
    }

    private void UpdateArmorRenderer(TInventorySlot item)
    {
        // @TO DO : Check if it's the right slot
        // if it is, update all visual related of this game object.

        // Equip
        if (item && item.ItemInSlot.Item is TItemArmor)
        {
            TItemArmor armor = item.ItemInSlot.Item as TItemArmor;

            if (armor.ArmorType == m_Type)
            {
                m_ArmorRenderer.sprite = armor.ItemSprite;
                m_LastArmor = armor;
            }
        }
    }
}
