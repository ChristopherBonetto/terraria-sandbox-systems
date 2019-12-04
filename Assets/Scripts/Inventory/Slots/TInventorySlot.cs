using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TInventorySlot : MonoBehaviour
{  
    public Sprite ItemSprite
    {
        get { return m_slotImage.sprite; }
        set { m_slotImage.sprite = value; }
    }

    public TItemQuantity ItemInSlot;
    public TInventory InventoryRef { get; private set; }

    [SerializeField] protected Image m_slotImage;

        
    public void InsertItemToSlot(TItemQuantity inItemToAdd)
    {
        ItemInSlot = inItemToAdd;
        m_slotImage.sprite = ItemInSlot.Item.ItemSprite;
        ShowImage(true);
    }
    
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

    public void DepleteAmount(int inDepletedAmount)
    {
        ItemInSlot.Amount -= inDepletedAmount;

        if (ItemInSlot.Amount <= 0)
            Clear();
    }

    public void Clear()
    {
        ItemInSlot = TItemQuantity.Empty;
        
        ShowImage(false);

        if (TItemHandler.SharedInstance.SelectedItem == this) TItemHandler.SharedInstance.SelectedItem = null;
    }

    public void ShowImage(bool inValue)
    {
        m_slotImage.gameObject.SetActive(inValue);
    }

    #region Drag and Drop event

    public virtual void FillThisSlot(TInventorySlot inSlot)
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
    #endregion

    public void TakeInventoryRef(TInventory inInventory)
    {
        InventoryRef = inInventory;
    }
}
