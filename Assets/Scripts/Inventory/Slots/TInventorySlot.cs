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

    [SerializeField] Image m_slotImage;

    private void OnEnable()
    {
        TItemHandler.OnDeselectEvent += FillThisSlot;
    }
    private void OnDisable()
    {
        TItemHandler.OnDeselectEvent -= FillThisSlot;
    }

    private void Start()
    {
        if(ItemInSlot.Item != null)
        {
            m_slotImage.sprite = ItemInSlot.Item.ItemSprite;
        }
    }
    
    public void InsertItemToSlot(TItemQuantity itemToAdd)
    {
        ItemInSlot = itemToAdd;
        m_slotImage.sprite = ItemInSlot.Item.ItemSprite;
        ShowImage(true);
    }
    

    public void SelectSlot()
    {
        TItemHandler.SharedInstance.SelectedItem = this;
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

        TItemHandler.SharedInstance.SelectedItem = null;
    }

    public void ShowImage(bool value)
    {
        m_slotImage.color = value ? Color.white : Color.clear;
    }

    #region Drag and Drop event

    public void FillThisSlot(TInventorySlot slot)
    {
        if(slot == this)
        {
            if (TInventory.Instance.InventoryIsOpen)
            {
                if (slot.ItemInSlot.Item == null)
                {
                    InsertItemToSlot(TItemHandler.SharedInstance.SelectedItem.ItemInSlot);
                    TItemHandler.SharedInstance.SelectedItem.ItemInSlot = TItemQuantity.Empty;
                    TItemHandler.SharedInstance.SelectedItem = null;
                    Debug.Log("inserisci");
                }
                else
                {
                    if (TItemHandler.SharedInstance.SelectedItem == this)
                    {
                        m_slotImage.sprite = ItemInSlot.Item.ItemSprite;
                        TItemHandler.SharedInstance.SelectedItem = null;
                        Debug.Log("riposa");
                    }
                    else
                    {
                        TItemQuantity tempSlot = this.ItemInSlot;

                        InsertItemToSlot(TItemHandler.SharedInstance.SelectedItem.ItemInSlot);

                        TItemHandler.SharedInstance.SelectedItem.InsertItemToSlot(tempSlot);

                        TItemHandler.SharedInstance.SelectedItem = null;
                    }
                }
            }
            else
            {
                TItemHandler.SharedInstance.SelectedItem = null;
            }
        }
        
    }
    #endregion

}
