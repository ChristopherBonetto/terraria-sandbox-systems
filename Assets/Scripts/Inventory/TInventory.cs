using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInventory : MonoBehaviour
{
    public static TInventory Instance;

    public List<TInventorySlot> InventorySlots = new List<TInventorySlot>();

    [SerializeField] private int m_slotsNumber;
    private int m_slotCounter = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InstantiateSlotsInInventory();
    }

    public void InstantiateSlotsInInventory()
    {
        if (m_slotCounter <= m_slotsNumber)
        {
            TInventorySlot tempSlotRef = UIManager.Instance.InstantiateSlotInInventory();

            AddSlotToInventory(tempSlotRef);
            
            m_slotCounter++;
            InstantiateSlotsInInventory();
        }
        else
        {
            m_slotCounter = 0;
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

    #region Collet New Item
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
            if(InventorySlots[i].ItemInSlot.Item != null)
            {
                if (InventorySlots[i].ItemInSlot.Item == addThisItem.Item)
                {
                    
                    InventorySlots[i].InsertItemToSlot(addThisItem);

                    Debug.Log("now you have " + InventorySlots[i].ItemInSlot.Item.ItemName + " : " + InventorySlots[i].ItemInSlot.Amount);

                    return true;
                }
            }
            
        }
        return false;
    }

    public void CheckFreeSlotAndCollect(TItemQuantity addThisItem)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if(InventorySlots[i].ItemInSlot.Item == null)
            {
                InventorySlots[i].InsertItemToSlot(addThisItem);

                return;
            }
        }
    }
    #endregion

    
}
