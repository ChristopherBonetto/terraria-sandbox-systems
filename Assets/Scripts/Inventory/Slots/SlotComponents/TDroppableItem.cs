using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TDroppableItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TInventorySlot m_slot;


    private void Awake()
    {
        m_slot = gameObject.GetComponent<TInventorySlot>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("used to open description pop-out");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("used to close description pop-out");
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(m_slot.ItemInSlot != null)
        {
            TInventorySlot originarySlot = eventData.pointerDrag.GetComponent<TInventorySlot>();

            if (originarySlot != null)
            {
                originarySlot.ItemInSlot = m_slot.ItemInSlot;
                UIManager.Instance.ChangeSpriteFromImage(originarySlot.m_slotImage, m_slot.ItemInSlot.StatsOfThisItem.Item.ItemSprite);
            }
            TItemHandler.Instance.DropItemAction(m_slot);
        }
        else
        {
            TItemHandler.Instance.DropItemAction(m_slot);
        }
    }

    
}
