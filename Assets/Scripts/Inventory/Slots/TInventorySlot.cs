using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//[RequireComponent(typeof(TDroppableItem), typeof(TDraggableItem))]
public class TInventorySlot : MonoBehaviour
{  
    public TItemQuantity ItemInSlot;

    public Image m_slotImage;


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
    
    

    public void SelectSlot()
    {
        TItemHandler.Instance.SelectSlot(this);
    }

    public void TakeItemFromThisSlot(TInventorySlot slot)
    {
        if(slot == this)
        {            
            TItemHandler.Instance.CurrentSelectedItem = this;            
        }
    }

    public void FillThisSlot(TInventorySlot slot)
    {
        TItemHandler.Instance.CurrentSelectedItem = null;
    }
}
