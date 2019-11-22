using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//[RequireComponent(typeof(TDroppableItem), typeof(TDraggableItem))]
public class TInventorySlot : MonoBehaviour
{  
    public TItemQuantity ItemInSlot;

    public Image m_slotImage;

    public TInventory InventoryRef;

    private void OnEnable()
    {
        TItemHandler.OnSelectEvent += TakeItemFromThisSlot;
        TItemHandler.OnDeselectEvent += FillThisSlot;
    }
    private void OnDisable()
    {
        TItemHandler.OnSelectEvent -= TakeItemFromThisSlot;
        TItemHandler.OnDeselectEvent -= FillThisSlot;
    }

    private void Awake()
    {
        m_slotImage = gameObject.GetComponent<Image>();
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
    }
    

    public void SelectSlot()
    {
        TItemHandler.Instance.SelectSlot(this);
    }

    #region Drag and Drop event
    public void TakeItemFromThisSlot(TInventorySlot slot)
    {
        if(slot == this)
        {
            if(ItemInSlot.Item != null)
            {
                TItemHandler.Instance.CurrentSelectedItem = this;            
            }
        }
    }

    public void FillThisSlot(TInventorySlot slot)
    {
        if(slot == this)
        {
            if (InventoryRef.InventoryIsOpen)
            {
                if (slot.ItemInSlot.Item == null)
                {
                    InsertItemToSlot(TItemHandler.Instance.CurrentSelectedItem.ItemInSlot);
                    TItemHandler.Instance.CurrentSelectedItem.ItemInSlot = TItemQuantity.Empty;
                    TItemHandler.Instance.CurrentSelectedItem = null;
                    Debug.Log("inserisci");
                }
                else
                {
                    if (TItemHandler.Instance.CurrentSelectedItem == this)
                    {
                        m_slotImage.sprite = ItemInSlot.Item.ItemSprite;
                        TItemHandler.Instance.CurrentSelectedItem = null;
                        Debug.Log("riposa");
                    }
                    else
                    {
                        TItemQuantity tempSlot = this.ItemInSlot;

                        InsertItemToSlot(TItemHandler.Instance.CurrentSelectedItem.ItemInSlot);

                        TItemHandler.Instance.CurrentSelectedItem.InsertItemToSlot(tempSlot);

                        TItemHandler.Instance.CurrentSelectedItem = null;
                        Debug.Log("scambia");
                    }
                }
            }
            else
            {
                if (TItemHandler.Instance.CurrentSelectedItem == this)
                {
                    TItemHandler.Instance.CurrentSelectedItem = null;
                    Debug.Log("deseleziona corrente");
                }
                else
                {
                    TItemHandler.Instance.CurrentSelectedItem = null;
                    SelectSlot();
                    Debug.Log("selezionato nuovo slot");
                }
            }
        }
        
    }
    #endregion

}
