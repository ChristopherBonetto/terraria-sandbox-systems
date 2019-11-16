using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TDraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler ,IEndDragHandler
{
    private TInventorySlot m_slot;


    private void Awake()
    {
        m_slot = gameObject.GetComponent<TInventorySlot>();
    }

    //This method called at the start of the grab check if grabbed item has an InventorySlot component; If this control returns true so call his method. 
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.pointerDrag.gameObject == this.gameObject)
        {
            if (m_slot != null)
            {
                m_slot.ChangeDraggedItemWithThisSlot();
            }
        }             
    }

    //Used to hold eventData reference.
    public void OnDrag(PointerEventData eventData)
    {

    }

    //Used to restore the state of the last grabbed item before being dragged.
    public void OnEndDrag(PointerEventData eventData)
    {
        if(TItemHandler.Instance.ItemDraggedInHand.Item != null)
        {
            TItemHandler.Instance.StopDragItemEvent();
        }
        else
        {
            Debug.Log("slot without item now");
        }
        TItemHandler.OnStopDragEvent -= m_slot.RestoreImageAndItemInSlot;
    }

    
}
