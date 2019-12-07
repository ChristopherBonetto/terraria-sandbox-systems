using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEquipmentSlot : TInventorySlot
{
    [Space, SerializeField] private ArmorType m_typeOfEquippableItem;

    
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
                TEventManager.TriggerEvent<TInventorySlot>(TEventID.OnItemEquipped, this as TInventorySlot);
                Debug.Log("equipped");
            }
            else
            {
                TEventManager.TriggerEvent<TInventorySlot>(TEventID.OnItemUnequipped, this as TInventorySlot);
                m_itemInSlot = value;
                Debug.Log("unequipped");
            }
            
        }
    }
    

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
}
