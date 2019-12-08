using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Class to manage the current player's item in hand.
public class TItemHandler : MonoBehaviour
{
    public static TItemHandler SharedInstance;

    #region Selected Item with events

    private TInventorySlot m_SelectedItem;

    //Changing the value of the Selected item, it will trigger a different event. 
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

    #endregion

    #region Events

    private void OnEnable()
    {
        TEventManager.SubscribeTo<bool>(TEventID.OnInventoryOpen, ResetSelectedItemWithEvent);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<bool>(TEventID.OnInventoryOpen, ResetSelectedItemWithEvent);
    }

    #endregion

    private void Awake()
    {
        SharedInstance = this;
    }

    //Used first of reset the current Selected item
    public void ResetSelectedItemWithEvent(bool inValue)
    {
        if(SelectedItem != null)
        {
            SelectedItem.ShowImage(true);
        }
        SelectedItem = null;
    }
}
