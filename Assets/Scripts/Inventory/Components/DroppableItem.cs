using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private InventorySlot m_slot;


    private void Awake()
    {
        m_slot = gameObject.GetComponent<InventorySlot>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(m_slot.ItemInSlot != null)
        {
            InventorySlot originarySlot = eventData.pointerDrag.GetComponent<InventorySlot>();

            if (originarySlot != null)
            {
                originarySlot.ItemInSlot = m_slot.ItemInSlot;
                originarySlot.m_slotImage.sprite = m_slot.ItemInSlot.ItemSprite;
            }
            ItemHandler.Instance.DropItemAction(m_slot);
        }
        else
        {
            ItemHandler.Instance.DropItemAction(m_slot);
        }
    }

    
}
