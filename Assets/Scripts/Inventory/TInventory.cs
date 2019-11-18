using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInventory : MonoBehaviour
{
    public static TInventory Instance;

    public List<TInventorySlot> InventorySlots = new List<TInventorySlot>();
    

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        DebugInventory();
    }

    public void CollectItem(TItem addThisItem)
    {
        if (!CheckSimilarItems(addThisItem))
        {
            CheckFreeSlotAndCollect(addThisItem);
        }
    }

    public bool CheckSimilarItems(TItem addThisItem)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if(InventorySlots[i].ItemInSlot != null)
            {
                if (InventorySlots[i].ItemInSlot.Item == addThisItem)
                {
                    InventorySlots[i].ItemInSlot.Amount += addThisItem.AmountGivenOnCollect;

                    Debug.Log("now you have " + InventorySlots[i].ItemInSlot.Item.ItemName + " : " + InventorySlots[i].ItemInSlot.Amount);

                    return true;
                }
            }
            
        }
        return false;
    }

    public void CheckFreeSlotAndCollect(TItem addThisItem)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if(InventorySlots[i].ItemInSlot == null)
            {
                InventorySlots[i].InsertItemToSlot(addThisItem);

                UIManager.Instance.ChangeSpriteFromImage(InventorySlots[i].m_slotImage, addThisItem.ItemSprite);

                return;
            }
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


    public void DebugInventory()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < InventorySlots.Count; i++)
            {
                if (InventorySlots[i].ItemInSlot == null)
                {
                    Debug.Log("Slot: " + i + " is empty!!!");
                }
                else
                {
                    Debug.Log("Slot: " + i + " have " + InventorySlots[i].ItemInSlot.Amount + " of " + InventorySlots[i].ItemInSlot.Item.ItemName);
                }
            }
        }
    }
    
}
