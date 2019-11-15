using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TItemHandler : MonoBehaviour
{
    public static TItemHandler Instance;

    public TItemQuantity itemInHand;

    #region Drag
    //Assign the itemInHand as another slotted item
    public delegate void OnBeginDragDelegate(TInventorySlot tempSlottedItem);
    public static OnBeginDragDelegate OnBeginDragEvent;

    public void StartDragItemEvent(TInventorySlot tempSlottedItem)
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
    public event Action<TInventorySlot> OnDropEventAction;

    public void DropItemAction(TInventorySlot itemToEquip)
    {
        if(OnDropEventAction != null)
        {
            if(itemInHand.Item != null)
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
        if(itemInHand.Item != null)
        {
            Debug.Log(itemInHand);
        }
        


        if (Input.GetKeyDown(KeyCode.A))
        {
            itemInHand.Item = null;
        }

    }
    

    public void TakeSlotFromHand(TInventorySlot slotToEquipItem)
    {
        slotToEquipItem.ItemInSlot = itemInHand;
        UIManager.Instance.ChangeSpriteFromImage(slotToEquipItem.m_slotImage, itemInHand.Item.ItemSprite);
        //slotToEquipItem.m_slotImage.sprite = itemInHand.ItemSprite;
        itemInHand.Item = null;
    }
}
