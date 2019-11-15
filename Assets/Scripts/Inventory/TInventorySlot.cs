using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(TDroppableItem), typeof(TDraggableItem))]
public class TInventorySlot : MonoBehaviour
{
    public TItemScriptable ItemInSlot = null;
    
    public Image m_slotImage;



    private void OnEnable()
    {
        TItemHandler.Instance.OnDropEventAction += ChangeSlottedItemWithHandItem;
    }
    private void OnDisable()
    {
        TItemHandler.Instance.OnDropEventAction -= ChangeSlottedItemWithHandItem;
    }




    private void Awake()
    {
        m_slotImage = gameObject.GetComponent<Image>();
    }


    private void Start()
    {
        if(ItemInSlot != null)
        {
            UIManager.Instance.ChangeSpriteFromImage(m_slotImage, ItemInSlot.ItemSprite);
        }
    }




    //Used to change the ItemHandler's variable itemInHand and subscribe the class of this item to another event.
    public void ChangeHandItemWithThisSlot()
    {   
        if(ItemInSlot != null)
        {
            TItemHandler.Instance.StartDragItemEvent(this);
            ItemInSlot = null;
            UIManager.Instance.ChangeSpriteFromImage(m_slotImage, null);

            TItemHandler.OnStopDragEvent += RestoreImageAndItemInSlot;
        }
    }



    //Used to restore the state of this class before being dragged.
    public void RestoreImageAndItemInSlot()
    {
        ItemInSlot = TItemHandler.Instance.itemInHand;
        UIManager.Instance.ChangeSpriteFromImage(m_slotImage, ItemInSlot.ItemSprite);
    }

    //Take the item in hand and equip it in this slot.
    public void ChangeSlottedItemWithHandItem(TInventorySlot slot)
    {
        if(slot == this)
        {
            TItemHandler.Instance.TakeSlotFromHand(this);
        }
    }

}
