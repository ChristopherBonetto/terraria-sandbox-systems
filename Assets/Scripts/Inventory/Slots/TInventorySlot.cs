using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TInventorySlot : MonoBehaviour, IShowDescription
{
    protected TItemQuantity m_itemInSlot;
    public virtual TItemQuantity ItemInSlot
    {
        get
        {
            return m_itemInSlot;
        }
        set
        {
            m_itemInSlot = value;
        }
    }

    public TInventory InventoryRef { get; protected set; }

    [SerializeField] protected Image m_slotImage;

    public Sprite ItemSprite
    {
        get { return m_slotImage.sprite; }
        set { m_slotImage.sprite = value; }
    }

    #region Slot's Events

    protected void OnEnable()
    {
        TEventManager.SubscribeTo<TInventory>(TEventID.OnInventoryCreated, TakeInventoryRef);
    }
    protected void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TInventory>(TEventID.OnInventoryCreated, TakeInventoryRef);
    }

    #endregion

    #region Button Action
    public virtual void SelectSlot()
    {
        if (TItemHandler.SharedInstance.SelectedItem)
            FillThisSlot(TItemHandler.SharedInstance.SelectedItem);

        else if (ItemInSlot != TItemQuantity.Empty)
        {
            TItemHandler.SharedInstance.SelectedItem = this;
            if (InventoryRef.m_isInventoryOpen) ShowImage(false);
        }
    }
    #endregion

    #region Fill slot

    protected virtual void FillThisSlot(TInventorySlot inSlot)
    {
        TInventorySlot tempSlot = TItemHandler.SharedInstance.SelectedItem;

        if(tempSlot != null)
        {
            if (InventoryRef.m_isInventoryOpen)
            {
                if (ItemInSlot.Item == null)
                {
                    InsertItemToSlot(tempSlot.ItemInSlot);
                    tempSlot.ShowImage(false);
                    tempSlot.ItemInSlot = TItemQuantity.Empty;
                }
                else
                {
                    if (tempSlot == this)
                    {
                        ShowImage(true);
                    }
                    else
                    {
                        TItemQuantity tempItem = this.ItemInSlot;
                        InsertItemToSlot(tempSlot.ItemInSlot);
                        tempSlot.InsertItemToSlot(tempItem);
                    }
                }
            }
            else
            {
                if (tempSlot != this)
                {
                    TItemHandler.SharedInstance.SelectedItem = null;
                    SelectSlot();
                }
                return;
            }
        }
        tempSlot = null;
        TItemHandler.SharedInstance.SelectedItem = tempSlot;
    }

    public void InsertItemToSlot(TItemQuantity inItemToAdd)
    {
        ItemInSlot = inItemToAdd;
        m_slotImage.sprite = ItemInSlot.Item.ItemSprite;
        ShowImage(true);
    }
    #endregion

    #region Methods to manage the item in slot
    public void ShowImage(bool inValue)
    {
        m_slotImage.gameObject.SetActive(inValue);
    }

    public void DepleteAmount(int inDepletedAmount)
    {
        m_itemInSlot.Amount -= inDepletedAmount;

        if (ItemInSlot.Amount <= 0)
            Clear();
    }

    public void Clear()
    {
        ItemInSlot = TItemQuantity.Empty;

        ShowImage(false);

        if (TItemHandler.SharedInstance.SelectedItem == this) TItemHandler.SharedInstance.SelectedItem = null;
    }

    #endregion



    protected virtual void TakeInventoryRef(TInventory inInventory)
    {
        InventoryRef = inInventory;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ItemInSlot != TItemQuantity.Empty && ItemInSlot.Item.TextToRead != null)
        {
            TEventManager.TriggerEvent<bool>(TEventID.OnOpenCloseDescription, true);
            TEventManager.TriggerEvent<TItem>(TEventID.OnSearchItem, this.ItemInSlot.Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TEventManager.TriggerEvent<bool>(TEventID.OnOpenCloseDescription, false);
        TEventManager.TriggerEvent<List<string>>(TEventID.OnShowTextDescription, null);
    }

    
}
