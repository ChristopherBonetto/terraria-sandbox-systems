using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
    }

    public void OnDrop(PointerEventData eventData)
    {        
        InventorySlot itemDragged = eventData.pointerDrag.GetComponent<InventorySlot>();

        InventorySlot slottedItem = transform.GetComponent<InventorySlot>();

        if (itemDragged != null)
        {
            Transform saveParent = null;
                        
            if (slottedItem)
            {
                saveParent = slottedItem.transform.parent;
                slottedItem.DraggableComponent.SetNewParent(itemDragged.DraggableComponent.m_parentToThisItem);
            }
            else
            {

            }

            itemDragged.DraggableComponent.m_parentToThisItem = saveParent;
            //Inventory.Instance.SwapItemsInList(itemDragged, slottedItem);
        }
    }
}
