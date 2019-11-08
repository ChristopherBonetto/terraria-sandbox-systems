using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptable ItemInSlot = null;

    public DraggableItem DraggableComponent;

    public DroppableItem DroppableComponent;

    public Image m_slotImage;

    

    private void Awake()
    {
        DraggableComponent = gameObject.GetComponent<DraggableItem>();
        DroppableComponent = gameObject.GetComponent<DroppableItem>();

        m_slotImage = gameObject.GetComponent<Image>();
        m_slotImage.sprite = ItemInSlot.ItemSprite;
    }

    public void CallChangeHandItem()
    {        
        ItemHandler.Instance.StartDragItemEvent(this.ItemInSlot);
    }

}
