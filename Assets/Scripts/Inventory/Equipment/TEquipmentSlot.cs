using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Inherits from TInventory slot because they have many common behaviours. 
public class TEquipmentSlot : TInventorySlot
{
    //Used to check which item can be slotted to this Slot.
    [Space, SerializeField] private ArmorType m_typeOfEquippableItem;

    #region Equip and Unequip events

    //Changing the item slotted from this Slot it will trigger a different event.
    public override TItemQuantity ItemInSlot
    {
        get
        {
            return m_itemInSlot;
        }
        set
        {
            if (m_itemInSlot == TItemQuantity.Empty)
            {
                m_itemInSlot = value;

                //After changing the slotted item equip it.
                TEventManager.TriggerEvent<TInventorySlot>(TEventID.OnItemEquipped, this as TInventorySlot);
            }
            else
            {
                //Disequip the slotted item and after change the value.
                TEventManager.TriggerEvent<TInventorySlot>(TEventID.OnItemUnequipped, this as TInventorySlot);
                m_itemInSlot = value;
            }
        }
    }

    #endregion

    #region Manage Slot

    /// <summary>
    /// After being selected, manage the items swap of this slot.
    /// First of all check if the item is a <param TItemArmor>.
    /// If the check returns true this method check if it can take this item and if it must swap two items.
    /// </summary>

    protected override void FillThisSlot(TInventorySlot inSlot)
    {
        if (inSlot.ItemInSlot.Item is TItemArmor)
        {
            TItemArmor tempArmor = inSlot.ItemInSlot.Item as TItemArmor;

            if(tempArmor != null)
            {
                if (tempArmor.ArmorType == m_typeOfEquippableItem)
                {
                    if(ItemInSlot.Item == null)
                    {
                        InsertItemToSlot(inSlot.ItemInSlot);
                        inSlot.ShowImage(false);
                        inSlot.ItemInSlot = TItemQuantity.Empty;
                    }
                    else
                    {
                        if(inSlot == this)
                        {
                            ShowImage(true);
                        }
                        else
                        {
                            TItemQuantity tempItem = this.ItemInSlot;
                            InsertItemToSlot(inSlot.ItemInSlot);
                            inSlot.InsertItemToSlot(tempItem);
                        }
                    }
                }
                else
                {
                    inSlot.ShowImage(true);
                }
            }
        }
        inSlot = null;
        TItemHandler.SharedInstance.SelectedItem = inSlot;
    }

    #endregion

    #region Event to the description with Interface

    /// <summary>
    /// If the mouse is over this item, it will trigger two events.
    /// First event Open the description panel with a offset.
    /// Second event is used to search the item's description into the Collection.
    /// </summary>
    /// <param name="eventData"> current obj which has the mouse over it </param>

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemInSlot != TItemQuantity.Empty && ItemInSlot.Item.TextToRead != null)
        {
            Vector3 positionOffset = new Vector3(gameObject.transform.position.x - 465, gameObject.transform.position.y - 50, gameObject.transform.position.z);

            TEventManager.TriggerEvent<bool, Vector3>(TEventID.OnOpenCloseDescription, true, positionOffset);
            TEventManager.TriggerEvent<TItem>(TEventID.OnSearchItem, this.ItemInSlot.Item);
        }
    }

    #endregion
}
