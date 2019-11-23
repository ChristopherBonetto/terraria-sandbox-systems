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
            m_SelectedItem = value;

            if (value)
                TEventManager.TriggerEvent(TEventID.OnItemSelected, value);
            else
                TEventManager.TriggerEvent(TEventID.OnItemDeselected);
        }
    }

    private TInventorySlot m_SelectedItem;

    private void Awake()
    {
        SharedInstance = this;
    }
}
