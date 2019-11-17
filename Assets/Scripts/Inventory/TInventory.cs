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
                if (InventorySlots[i].ItemInSlot.StatsOfThisItem.Item == addThisItem.StatsOfThisItem.Item)
                {
                    InventorySlots[i].ItemInSlot.StatsOfThisItem.Amount += addThisItem.StatsOfThisItem.Amount;

                    Debug.Log("now you have " + InventorySlots[i].ItemInSlot.StatsOfThisItem.Item.ItemName + " : " + InventorySlots[i].ItemInSlot.StatsOfThisItem.Amount);

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
                InventorySlots[i].ItemInSlot = addThisItem;
                UIManager.Instance.ChangeSpriteFromImage(InventorySlots[i].m_slotImage, addThisItem.StatsOfThisItem.Item.ItemSprite);

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


    
}
