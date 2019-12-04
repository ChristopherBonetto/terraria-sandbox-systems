using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TItemHandler : MonoBehaviour
{
    public static TItemHandler SharedInstance;

    public TInventorySlot SelectedItem
    {
        get { return m_SelectedItem; }
        set
        {
            if (value)
                TEventManager.TriggerEvent(TEventID.OnItemSelected, value);
            else
                TEventManager.TriggerEvent(TEventID.OnItemDeselected, m_SelectedItem);

            m_SelectedItem = value;

        }
    }

    private TInventorySlot m_SelectedItem;


    private void OnEnable()
    {
        TEventManager.SubscribeTo<bool>(TEventID.OnInventoryOpen, ResetSelectedItemWithEvent);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<bool>(TEventID.OnInventoryOpen, ResetSelectedItemWithEvent);
    }



    private void Awake()
    {
        SharedInstance = this;
    }

    public void ResetSelectedItemWithEvent(bool inValue)
    {
        if(SelectedItem != null)
        {
            SelectedItem.ShowImage(true);
        }
        SelectedItem = null;
    }
}
