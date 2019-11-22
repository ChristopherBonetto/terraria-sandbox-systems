using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItemInHandComponent : MonoBehaviour
{
    public TItemQuantity m_myItemInHand { get; private set; }

    private void OnEnable()
    {
        TItemHandler.OnSelectEvent += ChangeItemInHand;
    }
    private void OnDisable()
    {
        TItemHandler.OnSelectEvent -= ChangeItemInHand;
    }
        

    public void ChangeItemInHand(TInventorySlot newItem)
    {
        m_myItemInHand = newItem.ItemInSlot;
    }
    
}
