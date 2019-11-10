using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHandler : MonoBehaviour
{
    public static ItemHandler Instance;

    public ItemScriptable itemInHand = null;

    #region Drag
    //Assign the itemInHand as another slotted item
    public delegate void OnBeginDragDelegate(InventorySlot tempSlottedItem);
    public static OnBeginDragDelegate OnBeginDragEvent;

    public void StartDragItemEvent(InventorySlot tempSlottedItem)
    {
        if(tempSlottedItem != null)
        {
            itemInHand = tempSlottedItem.ItemInSlot;
            OnDragEvent();
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
    public event Action<InventorySlot> OnDropEventAction;

    public void DropItemAction(InventorySlot itemToEquip)
    {
        if(OnDropEventAction != null)
        {
            OnDropEventAction(itemToEquip);
            DropEvent();
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

    private void OnEnable()
    {
        OnBeginDragEvent = StartDragItemEvent;
        
    }
    private void OnDisable()
    {
        OnBeginDragEvent = StartDragItemEvent;
        
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }
    

    void Update()
    {
        if(itemInHand != null)
        {
            Debug.Log(itemInHand);
        }
        


        if (Input.GetKeyDown(KeyCode.A))
        {
            itemInHand = null;
        }

    }
    

    public void TakeSlotFromHand(InventorySlot slotToEquipItem)
    {
        slotToEquipItem.ItemInSlot = itemInHand;
        slotToEquipItem.m_slotImage.sprite = itemInHand.ItemSprite;
        itemInHand = null;
    }
}
