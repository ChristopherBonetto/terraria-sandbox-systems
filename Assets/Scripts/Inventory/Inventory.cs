using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventorySlot> InventorySlots = new List<InventorySlot>();
    private int m_inventoryCount = 0;

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CollectedItem();
        } 

        
    }


    public void CollectedItem()
    {
        if (m_inventoryCount <= InventorySlots.Count)
        {
            if(InventorySlots[m_inventoryCount].ItemInSlot == null)
            {
                Debug.Log(m_inventoryCount + " addItem ");
            }
            else
            {
                m_inventoryCount++;
                CollectedItem();
            }
            
        }
        else
        {
            m_inventoryCount = 0;
            return;
        }
    }

    public void AddToInventory(InventorySlot inSlotToAdd)
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

    //public int? GiveIndex(InventorySlot inItemInInventory)
    //{
    //    if (InventorySlots.Contains(inItemInInventory))
    //    {
    //        int index = InventorySlots.FindIndex(d => d == inItemInInventory);
    //        return index;
    //    }
    //    else
    //    {
    //        return null;
    //    }
        
    //}

    //public void SwapItemsInList(InventorySlot inFirstItem, InventorySlot inSecondItem)
    //{
    //    int Aindex = 0;
    //    int Bindex = 0;

    //    if (InventorySlots.Contains(inFirstItem))
    //    {
    //        Aindex = InventorySlots.FindIndex(d => d == inFirstItem);
            
    //    }


    //    int indexFirstItem = GiveIndex(inFirstItem) ?? 0;

    //    int indexSecondItem = GiveIndex(inSecondItem) ?? 0;

    //    Debug.Log(indexFirstItem + " " + indexSecondItem);

    //    InventorySlot a = new InventorySlot();
        

    //    InventorySlots[indexFirstItem] = InventorySlots[indexSecondItem];
    //    InventorySlots[indexSecondItem] = InventorySlots[indexFirstItem];


    //    int test = GiveIndex(inFirstItem) ?? 0;

    //    Debug.Log(test);
        
        
        
    //}


    
}
