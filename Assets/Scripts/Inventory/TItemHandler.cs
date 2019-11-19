using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TItemHandler : MonoBehaviour
{
    public static TItemHandler Instance;
    
    public TInventorySlot CurrentSelectedItem = null;

    public delegate void OnSelect(TInventorySlot tempSlottedItem);
    public static OnSelect OnSelectEvent;
    public static OnSelect OnDeselectEvent;

    public void SelectSlot(TInventorySlot tempSlottedItem)
    {
        if(CurrentSelectedItem == null)
        {
            if (OnSelectEvent != null)
            {
                OnSelectEvent(tempSlottedItem);
            }
        }
        else
        {
            if (OnDeselectEvent != null)
            {
                OnDeselectEvent(tempSlottedItem);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

   
}
