using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TItemHandler : MonoBehaviour
{
    public static TItemHandler Instance;

    public TItemQuantity ItemDraggedInHand;
    public TInventorySlot CurrentSelectedItem = null;

    #region Drag
    //Assign the itemInHand as another slotted item
    public delegate void OnBeginDragDelegate(TInventorySlot tempSlottedItem);
    public static OnBeginDragDelegate OnBeginDragEvent;

    public void StartDragItemEvent(TInventorySlot tempSlottedItem)
    {
        if(tempSlottedItem != null)
        {            
            if (tempSlottedItem.ItemInSlot != null)
            {
                ItemDraggedInHand = tempSlottedItem.ItemInSlot;
                OnDragEvent();
            }
        }
    }

    //Start to drag the item
    public delegate void OnDragDelegate();
    public static OnDragDelegate OnDragEvent;

    public void DragItemEvent()
    {
        if(OnDragEvent != null)
        {
            OnDragEvent();
        }
    }

    // **Chiedere in classe se e meglio fare un action come sotto oppure iscriverlo e disiscriverlo dall'evento ogni volta.
    public delegate void OnStopDragDelegate();
    public static OnStopDragDelegate OnStopDragEvent;

    public void StopDragItemEvent()
    {
        if (OnStopDragEvent != null)
        {
            OnStopDragEvent();
        }
    }
    #endregion

    #region Drop
    //Used to drop an item to the selected slot
    public event Action<TInventorySlot> OnDropEventAction;

    public void DropItemAction(TInventorySlot itemToEquip)
    {
        if(OnDropEventAction != null)
        {
            if(ItemDraggedInHand.Item != null)
            {                
                OnDropEventAction(itemToEquip);
                DropEvent();
            }
        }
    }

    
    public delegate void OnDropDelegate();
    public static OnDropDelegate OnDropEvent;

    public void DropEvent()
    {
        if (OnDropEvent != null)
        {
            OnDropEvent();
        }
    }
    #endregion

    public event Action<TInventorySlot> OnSelectedSlotEventAction;

    public void SelectedSlotAction(TInventorySlot slotToSelect)
    {
        if (OnSelectedSlotEventAction != null)
        {
            if(CurrentSelectedItem == null)
            {
                OnSelectedSlotEventAction(slotToSelect);
            }
            else
            {
                UIManager.Instance.ChangeColorFromImage(CurrentSelectedItem.m_slotImage, Color.white);
                OnSelectedSlotEventAction(slotToSelect);
            }            
        }
    }

    private void OnEnable()
    {
        OnBeginDragEvent = StartDragItemEvent;

        OnDragEvent += DeselectCurrentSelectedItem;
    }

    private void OnDisable()
    {
        OnDragEvent -= DeselectCurrentSelectedItem;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CurrentSelectedItem = null;
        }
    }

    public void TakeSlotFromHand(TInventorySlot slotToEquipItem)
    {
        slotToEquipItem.ItemInSlot = ItemDraggedInHand;
        UIManager.Instance.ChangeSpriteFromImage(slotToEquipItem.m_slotImage, ItemDraggedInHand.Item.ItemSprite);
        ItemDraggedInHand = null;
    }

    public void DeselectCurrentSelectedItem()
    {
        if(CurrentSelectedItem != null)
        {
            UIManager.Instance.RestoreColorToSelectedSlotWhenDragged();
            CurrentSelectedItem = null;
        }
    }
}
