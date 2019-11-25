using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItemInHandComponent : MonoBehaviour
{
    public TInventorySlot ItemInHand;

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemSelected, ChangeItem);
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemDeselected, ClearItem);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemSelected, ChangeItem);
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemDeselected, ClearItem);
    }
        
    public void ChangeItem(TInventorySlot inItem)
    {
        ItemInHand = inItem;
    }

    public void ClearItem(TInventorySlot inItem)
    {
        ItemInHand = null;
    } 
}
