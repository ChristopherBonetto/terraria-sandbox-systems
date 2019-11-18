using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(TDroppableItem), typeof(TDraggableItem))]
public class TInventorySlot : MonoBehaviour
{
    public TItemQuantity ItemInSlot = null;
    
    public Image m_slotImage;
    

    private void Update()
    {
        
    }


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
        if(ItemInSlot != null)
        {
            UIManager.Instance.ChangeSpriteFromImage(m_slotImage, ItemInSlot.Item.ItemSprite);
            
        }
    }
    



    //Used from button click.
    public void SelectThisSlotForEvent()
    {
        if(ItemInSlot != null)
        {
            if (ItemInSlot.Item != null && TItemHandler.Instance.CurrentSelectedItem != this)
            {
                TItemHandler.Instance.SelectedSlotAction(this);
            }
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
        if(ItemInSlot != null)
        {
            if (ItemInSlot.Item != null)
            {
                TItemHandler.Instance.StartDragItemEvent(this);
                ItemInSlot = null;
                UIManager.Instance.ChangeSpriteFromImage(m_slotImage, null);

                TItemHandler.OnStopDragEvent += RestoreImageAndItemInSlot;
            }
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
    
    public void InsertItemToSlot(TItem addThisItem)
    {
        ItemInSlot = new TItemQuantity(addThisItem, addThisItem.AmountGivenOnCollect);
    }
}
