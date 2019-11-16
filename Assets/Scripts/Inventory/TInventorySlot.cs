using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(TDroppableItem), typeof(TDraggableItem))]
public class TInventorySlot : MonoBehaviour
{
    public TItemQuantity ItemInSlot;
    
    public Image m_slotImage;

    

    private void OnEnable()
    {
        TItemHandler.Instance.OnDropEventAction += ChangeSlottedItemWithDraggedHandItem;

        TItemHandler.Instance.OnSelectedSlotEventAction += StartSelectedSlotEvent;
    }
    private void OnDisable()
    {
        TItemHandler.Instance.OnDropEventAction -= ChangeSlottedItemWithDraggedHandItem;

        TItemHandler.Instance.OnSelectedSlotEventAction -= StartSelectedSlotEvent;
    }




    private void Awake()
    {
        m_slotImage = gameObject.GetComponent<Image>();
    }


    private void Start()
    {
        if(ItemInSlot.Item != null)
        {
            UIManager.Instance.ChangeSpriteFromImage(m_slotImage, ItemInSlot.Item.ItemSprite);
        }
    }
    




    public void SelectThisSlotForEvent()
    {
        if (ItemInSlot.Item != null && TItemHandler.Instance.CurrentSelectedItem != this)
        {
            StartSelectedSlotEvent(this);
        }
    }

    public void StartSelectedSlotEvent(TInventorySlot slot)
    {
        if(slot == this)
        {
            TItemHandler.Instance.CurrentSelectedItem = this;
            UIManager.Instance.ChangeColorFromImage(m_slotImage, Color.green);
        }
                
    }

    //Used to change the ItemHandler's variable itemInHand and subscribe the class of this item to another event.
    public void ChangeDraggedItemWithThisSlot()
    {   
        if(ItemInSlot.Item != null)
        {
            TItemHandler.Instance.StartDragItemEvent(this);
            ItemInSlot.Item = null;
            UIManager.Instance.ChangeSpriteFromImage(m_slotImage, null);

            TItemHandler.OnStopDragEvent += RestoreImageAndItemInSlot;
        }
    }



    //Used to restore the state of this class before being dragged.
    public void RestoreImageAndItemInSlot()
    {
        ItemInSlot = TItemHandler.Instance.ItemDraggedInHand;
        UIManager.Instance.ChangeSpriteFromImage(m_slotImage, ItemInSlot.Item.ItemSprite);
    }

    //Take the item in hand and equip it in this slot.
    public void ChangeSlottedItemWithDraggedHandItem(TInventorySlot slot)
    {
        if(slot == this)
        {
            TItemHandler.Instance.TakeSlotFromHand(this);
        }
    }

}
