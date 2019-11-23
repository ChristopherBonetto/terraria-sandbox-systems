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
    public TInventory InventoryRef;

    [SerializeField] Image m_slotImage;

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
        if (TItemHandler.SharedInstance.SelectedItem)
            FillThisSlot(TItemHandler.SharedInstance.SelectedItem);

        else if (ItemInSlot != TItemQuantity.Empty)
        {
            TItemHandler.SharedInstance.SelectedItem = this;
            if (UIManager.SharedInstance.IsInventoryOpen) ShowImage(false);
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

    public void ShowImage(bool value)
    {
        m_slotImage.gameObject.SetActive(value);
    }

    #region Drag and Drop event

    public void FillThisSlot(TInventorySlot slot)
    {
        if (InventoryRef.InventoryIsOpen)
        {
            if (ItemInSlot.Item == null)
            {
                InsertItemToSlot(TItemHandler.SharedInstance.SelectedItem.ItemInSlot);
                TItemHandler.SharedInstance.SelectedItem.ShowImage(false);
                TItemHandler.SharedInstance.SelectedItem.ItemInSlot = TItemQuantity.Empty;
                TItemHandler.SharedInstance.SelectedItem = null;
                Debug.Log("inserisci");
            }
            else
            {
                if (TItemHandler.SharedInstance.SelectedItem == this)
                {
                    ShowImage(true);
                    TItemHandler.SharedInstance.SelectedItem = null;
                    Debug.Log("riposa");
                }
                else
                {
                    TItemQuantity tempSlot = this.ItemInSlot;

                    InsertItemToSlot(TItemHandler.SharedInstance.SelectedItem.ItemInSlot);

                    TItemHandler.SharedInstance.SelectedItem.InsertItemToSlot(tempSlot);

                    TItemHandler.SharedInstance.SelectedItem = null;
                    Debug.Log("scambia");
                }
            }
        }
        else
        {
            if (TItemHandler.SharedInstance.SelectedItem == this)
            {
                TItemHandler.SharedInstance.SelectedItem = null;
                Debug.Log("deseleziona corrente");
            }
            else
            {
                TItemHandler.SharedInstance.SelectedItem = null;
                SelectSlot();
                Debug.Log("selezionato nuovo slot");
            }
        }
    }
    #endregion

}
