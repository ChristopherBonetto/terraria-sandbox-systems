using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInventory : MonoBehaviour
{
    public List<TInventorySlot> InventorySlots = new List<TInventorySlot>();

    [SerializeField] private int m_slotsNumber;
    private int m_slotCounter = 0;

    public bool InventoryIsOpen = false;

    

    private void Start()
    {
        InstantiateSlotsInInventory();

        ChangeOpenCloseInventoryBool(InventoryIsOpen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryIsOpen = !InventoryIsOpen;
            ChangeOpenCloseInventoryBool(InventoryIsOpen);
        }
    }

    //Used to open and close Inventory
    public void ChangeOpenCloseInventoryBool(bool inIsOpen)
    {
        TItemHandler.SharedInstance.SelectedItem = null;
        UIManager.SharedInstance.OpenCloseInventory(inIsOpen);
    }

    
    #region Start Create Inventory
    public void InstantiateSlotsInInventory()
    {
        if (m_slotCounter <= m_slotsNumber)
        {
            TInventorySlot tempSlotRef = UIManager.SharedInstance.InstantiateSlotInInventory();
            tempSlotRef.InventoryRef = this;

            AddSlotToInventory(tempSlotRef);
            
            m_slotCounter++;
            InstantiateSlotsInInventory();
        }
        else
        {
            m_slotCounter = 0;

            InventoryIsOpen = false;
            
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
    #endregion

    #region Collet New Item
    public void CollectItem(TItemQuantity inAddThisItem)
    {
        if (!CheckSimilarItems(inAddThisItem))
        {
            CheckFreeSlotAndCollect(inAddThisItem);
        }
    }

    public bool CheckSimilarItems(TItemQuantity inAddThisItem)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if(InventorySlots[i].ItemInSlot.Item != null)
            {
                if (InventorySlots[i].ItemInSlot.Item == inAddThisItem.Item)
                {

                    InventorySlots[i].ItemInSlot.Amount += inAddThisItem.Amount;

                    Debug.Log("now you have " + InventorySlots[i].ItemInSlot.Item.ItemName + " : " + InventorySlots[i].ItemInSlot.Amount);

                    return true;
                }
            }
            
        }
        return false;
    }

    public void CheckFreeSlotAndCollect(TItemQuantity inAddThisItem)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if(InventorySlots[i].ItemInSlot.Item == null)
            {
                InventorySlots[i].InsertItemToSlot(inAddThisItem);
                
                return;
            }
        }
    }
    #endregion


    public List<TItem> ItemsInInventory()
    {
        List<TItem> tempItemsList = new List<TItem>();

        for(int i = 0; i < InventorySlots.Count; i++)
        {
            if (InventorySlots[i].ItemInSlot != TItemQuantity.Empty)
            {
                Debug.Log(InventorySlots[i].ItemInSlot.Item.ItemName);
                tempItemsList.Add(InventorySlots[i].ItemInSlot.Item);
            }
        }
        return tempItemsList;
    }
}
