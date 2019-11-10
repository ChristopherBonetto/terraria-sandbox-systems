using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventorySlot m_slot;


    private void Awake()
    {
        m_slot = gameObject.GetComponent<InventorySlot>();
    }

    //This method called at the start of the grab check if grabbed item has an InventorySlot component; If this control returns true so call his method. 
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.pointerDrag.gameObject == this.gameObject)
        {
            if (m_slot != null)
            {
                m_slot.ChangeHandItemWithThisSlot();
            }
        }
        //InventorySlot tempSlot = eventData.pointerDrag.GetComponent<InventorySlot>();

        //if(tempSlot != null)
        //{
        //    tempSlot.ChangeHandItemWithThisSlot();
        //}
             
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }


    //Used to restore the state of the last grabbed item before being dragged.
    public void OnEndDrag(PointerEventData eventData)
    {
        if(ItemHandler.Instance.itemInHand != null)
        {
            ItemHandler.Instance.StopDragItemEvent();
        }
        else
        {
            Debug.Log("slot without item now");
        }
        ItemHandler.OnStopDragEvent -= m_slot.RestoreImageAndItemInSlot;
    }
    
}
