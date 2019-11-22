using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItemInHandComponent : MonoBehaviour
{
    public TInventorySlot ItemInHand { get; private set; }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemSelected, ChangeItem);
        TEventManager.SubscribeTo(TEventID.OnSelectedItemCleared, ClearItem);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemSelected, ChangeItem);
        TEventManager.UnsubscribeFrom(TEventID.OnSelectedItemCleared, ClearItem);
    }
        
    public void ChangeItem(TInventorySlot newItem)
    {
        ItemInHand = newItem;
    }

    public void ClearItem()
    {
        ItemInHand = null;
    } 
}
