using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEquipmentSlot : TInventorySlot
{
    [Space, SerializeField] ArmorType m_typeOfEquippableItem;


    public override void SelectSlot()
    {
        if (TItemHandler.SharedInstance.SelectedItem)
            FillThisSlot(TItemHandler.SharedInstance.SelectedItem);

        else if (ItemInSlot != TItemQuantity.Empty)
        {
            TItemHandler.SharedInstance.SelectedItem = this;
            ShowImage(false);
        }
    }


    public override void FillThisSlot(TInventorySlot inSlot)
    {
        if (inSlot.ItemInSlot.Item is TItemArmor)
        {
            TItemArmor tempArmor = inSlot.ItemInSlot.Item as TItemArmor;

            if(tempArmor != null)
            {
                if (tempArmor.ArmorType == m_typeOfEquippableItem)
                {
                    InsertItemToSlot(inSlot.ItemInSlot);
                    inSlot.ShowImage(false);
                    inSlot.ItemInSlot = TItemQuantity.Empty;
                }
            }
        }
        inSlot = null;
        TItemHandler.SharedInstance.SelectedItem = inSlot;
    }
}
