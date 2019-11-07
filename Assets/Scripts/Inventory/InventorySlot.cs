using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptable ItemInSlot = null;

    public DraggableItem DraggableComponent;

    public DroppableItem DroppableComponent;



    private void Awake()
    {
        DraggableComponent = gameObject.GetComponent<DraggableItem>();
        DroppableComponent = gameObject.GetComponent<DroppableItem>();
    }

    
}
