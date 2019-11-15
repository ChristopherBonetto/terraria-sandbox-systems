using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInventory : MonoBehaviour
{
    public static TInventory Instance;

    public List<TInventorySlot> InventorySlots = new List<TInventorySlot>();
    private int m_inventoryCount = 0;
    

    private void Awake()
    {
        Instance = this;
    }


    


    public void CheckFreeSlotAndCollect(TItemQuantity addThisItem)
    {
        if (m_inventoryCount <= InventorySlots.Count)
        {
            if(InventorySlots[m_inventoryCount].ItemInSlot.Item == null)
            {
                InventorySlots[m_inventoryCount].ItemInSlot.Item = addThisItem.Item;

                UIManager.Instance.ChangeSpriteFromImage(InventorySlots[m_inventoryCount].m_slotImage, addThisItem.Item.ItemSprite);
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

    public void AddSlotToInventory(TInventorySlot inSlotToAdd)
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
