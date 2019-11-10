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



    private void OnEnable()
    {
        ItemHandler.Instance.OnDropEventAction += ChangeSlottedItemWithHandItem;
    }
    private void OnDisable()
    {
        ItemHandler.Instance.OnDropEventAction -= ChangeSlottedItemWithHandItem;
    }




    private void Awake()
    {
        DraggableComponent = gameObject.GetComponent<DraggableItem>();
        DroppableComponent = gameObject.GetComponent<DroppableItem>();

        m_slotImage = gameObject.GetComponent<Image>();

        //implement a ui'method that it do that with parameters.
        m_slotImage.sprite = ItemInSlot.ItemSprite;
    }




    //Used to change the ItemHandler's variable itemInHand and subscribe the class of this item to another event.
    public void ChangeHandItemWithThisSlot()
    {        
        ItemHandler.Instance.StartDragItemEvent(this);
        ItemInSlot = null;

        //implement a ui'method that it do that with parameters.
        m_slotImage.sprite = null;

        ItemHandler.OnStopDragEvent += RestoreImageAndItemInSlot;
    }



    //Used to restore the state of this class before being dragged.
    public void RestoreImageAndItemInSlot()
    {
        ItemInSlot = ItemHandler.Instance.itemInHand;
        m_slotImage.sprite = ItemInSlot.ItemSprite;
    }

    //Take the item in hand and equip it in this slot.
    public void ChangeSlottedItemWithHandItem(InventorySlot slot)
    {
        if(slot == this)
        {
            ItemHandler.Instance.TakeSlotFromHand(this);
                        
            
        }
    }

}
