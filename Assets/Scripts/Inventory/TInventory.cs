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

    public void CollectItem(TItemQuantity addThisItem)
    {
        if (!CheckSimilarItems(addThisItem))
        {
            CheckFreeSlotAndCollect(addThisItem);
        }
    }

    public bool CheckSimilarItems(TItemQuantity addThisItem)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if (InventorySlots[i].ItemInSlot.Item == addThisItem.Item)
            {
                InventorySlots[i].ItemInSlot.Amount += addThisItem.Amount;

                Debug.Log("now you have " + InventorySlots[i].ItemInSlot.Item.ItemName + " : " + InventorySlots[i].ItemInSlot.Amount);

                return true;
            }
        }
        return false;
    }

    public void CheckFreeSlotAndCollect(TItemQuantity addThisItem)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if (InventorySlots[i].ItemInSlot.Item == null)
            {
                InventorySlots[i].ItemInSlot = addThisItem;
                UIManager.Instance.ChangeSpriteFromImage(InventorySlots[i].m_slotImage, addThisItem.Item.ItemSprite);

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
