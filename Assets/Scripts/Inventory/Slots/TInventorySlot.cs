using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class to manage the single inventory's slot.
/// This class implement the interface <param IShowDescription> to trigger event when the mouse is over this item.
/// </summary>
public class TInventorySlot : MonoBehaviour, IShowDescription
{
    //The current item slotted
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

            if (m_AmountText) m_AmountText.text = value.Amount > 0 ? value.Amount.ToString() : string.Empty;
        }
    }

    //Reference to the player's inventory.
    public TInventory InventoryRef { get; protected set; }

    public Sprite ItemSprite
    {
        get { return m_slotImage.sprite; }
        set { m_slotImage.sprite = value; }
    }

    //To manage the current sprite of this slot.
    [SerializeField] protected Image m_slotImage;

    [SerializeField] protected Text m_AmountText;
    
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

    #region Manage Slot

    /// <summary>
    /// Used on Click of this button.
    /// This method check the <param SelectedItem in TItemHandler> value.
    /// If the selectedItem isn't null fill this slot else fill the selected item with this slot.
    /// </summary>
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

    /// <summary>
    /// This method is used to fill this slot with a <param inSlot = TInventorySlot>, checking if is empy or not.
    /// If this slot isn't empty so swap the contents of two slots.
    /// </summary>
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

    /// <summary>
    /// Used to insert another item into this slot.
    /// </summary>
    public void InsertItemToSlot(TItemQuantity inItemToAdd)
    {
        ItemInSlot = inItemToAdd;
        m_slotImage.sprite = ItemInSlot.Item.ItemSprite;
        if (m_AmountText) m_AmountText.text = ItemInSlot.Amount.ToString();
        ShowImage(true);
    }

    #endregion

    #region Minor methods to manage the item in slot

    public void ShowImage(bool inValue)
    {
        m_slotImage.gameObject.SetActive(inValue);
        if (m_AmountText) m_AmountText.gameObject.SetActive(inValue);
    }

    /// <summary>
    /// Used to reduces the amount of the current item in slot.
    /// </summary>
    /// <param name="inDepletedAmount"> amount to deplete </param>
    public void DepleteAmount(int inDepletedAmount)
    {
        m_itemInSlot.Amount -= inDepletedAmount;

        if (ItemInSlot.Amount <= 0)
            Clear();
        else if (m_AmountText)
            m_AmountText.text = m_itemInSlot.Amount.ToString();
    }

    public void Clear()
    {
        ItemInSlot = TItemQuantity.Empty;

        ShowImage(false);

        if (TItemHandler.SharedInstance.SelectedItem == this) TItemHandler.SharedInstance.SelectedItem = null;

        if (m_AmountText) m_AmountText.text = string.Empty;
    }

    #endregion


    //Fill the inventory reference with the inventory that it created this slot.
    protected virtual void TakeInventoryRef(TInventory inInventory)
    {
        InventoryRef = inInventory;
    }

    #region Trigger events when mouse is over this slot

    /// <summary>
    /// If the mouse is over this item, it will trigger two events.
    /// First event Open the description panel with a offset.
    /// Second event is used to search the item's description into the Collection.
    /// </summary>
    /// <param name="eventData"> current obj which has the mouse over it </param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(ItemInSlot != TItemQuantity.Empty && ItemInSlot.Item.TextToRead != null)
        {
            Vector3 positionOffset = new Vector3(gameObject.transform.position.x + 40, gameObject.transform.position.y - 40, gameObject.transform.position.z);

            TEventManager.TriggerEvent<bool, Vector3>(TEventID.OnOpenCloseDescription, true, positionOffset);
            TEventManager.TriggerEvent<TItem>(TEventID.OnSearchItem, this.ItemInSlot.Item);
        }
    }

    /// <summary>
    /// When the mouse leaves this slots, it will trigger two events.
    /// First event close the description panel.
    /// Second event is used to reset the description.
    /// </summary>
    /// <param name="eventData"> current obj which has the mouse over it </param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemInSlot != TItemQuantity.Empty)
        {
            TEventManager.TriggerEvent<bool, Vector3>(TEventID.OnOpenCloseDescription, false, Vector2.zero);
            TEventManager.TriggerEvent<List<string>>(TEventID.OnShowTextDescription, null);
        }
    }

    #endregion
}
