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

    public void ChangeHandItemWithThisSlot()
    {        
        ItemHandler.Instance.StartDragItemEvent(this);
        ItemInSlot = null;
        m_slotImage.sprite = null;

        ItemHandler.OnStopDragEvent += TakeImageAndItem;
    }


    public void TakeImageAndItem()
    {
        ItemInSlot = ItemHandler.Instance.itemInHand;
        m_slotImage.sprite = ItemInSlot.ItemSprite;

        ItemHandler.OnStopDragEvent -= TakeImageAndItem;
    }

}
