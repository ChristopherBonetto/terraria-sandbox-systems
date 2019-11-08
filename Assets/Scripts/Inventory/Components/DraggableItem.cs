using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        InventorySlot tempSlot = eventData.pointerDrag.GetComponent<InventorySlot>();

        if(tempSlot != null)
        {
            tempSlot.ChangeHandItemWithThisSlot();
        }
             
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemHandler.Instance.StopDragItemEvent();
    }
    
}
