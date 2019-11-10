using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventorySlot> InventorySlots = new List<InventorySlot>();
    private int m_inventoryCount = 0;

    public ItemScriptable itemToAdd;

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(itemToAdd != null)
            CheckFreeSlotAndCollect(itemToAdd);
        } 
    }


    public void CheckFreeSlotAndCollect(ItemScriptable addThisItem)
    {
        if (m_inventoryCount <= InventorySlots.Count)
        {
            if(InventorySlots[m_inventoryCount].ItemInSlot == null)
            {
                InventorySlots[m_inventoryCount].ItemInSlot = addThisItem;
                InventorySlots[m_inventoryCount].m_slotImage.sprite = addThisItem.ItemSprite;
            }
            else
            {
                m_inventoryCount++;
                CheckFreeSlotAndCollect(addThisItem);
            }
            
        }
        else
        {
            m_inventoryCount = 0;
            return;
        }
    }

    public void AddSlotToInventory(InventorySlot inSlotToAdd)
    {
        if (!InventorySlots.Contains(inSlotToAdd))
        {
            InventorySlots.Add(inSlotToAdd);
        }
        else
        {
            Debug.Log("the list contains this element");
        }
    }


    
}
