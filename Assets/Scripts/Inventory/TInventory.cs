using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInventory : MonoBehaviour
{
    private TPlayerController m_myPlayer;

    [SerializeField] private List<TItemQuantity> StartingItems = new List<TItemQuantity>();
    public List<TInventorySlot> InventorySlots { get; private set; } = new List<TInventorySlot>();

    public bool m_isInventoryOpen { get; private set; }




    private void OnEnable()
    {
        TEventManager.SubscribeTo<bool>(TEventID.OnInventoryOpen, ChangeOpenCloseInventoryBool);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<bool>(TEventID.OnInventoryOpen, ChangeOpenCloseInventoryBool);
    }






    private void Awake()
    {
        m_myPlayer = gameObject.GetComponent<TPlayerController>();
    }

    private void Start()
    {
        StartCoroutine(CollectStartingItems());
    }


    public IEnumerator CollectStartingItems()
    {
        yield return new WaitForEndOfFrame();

        int counter = 0;

        while(counter < StartingItems.Count)
        {
            CollectItem(StartingItems[counter]);
            counter++;
        }
    }


    public void InventorySlotsReference()
    {
        List<GameObject> tempInventorySlotsList = new List<GameObject>();
        tempInventorySlotsList = ObjectPooler.SharedInstance.ReturnListFromDictionary("InventorySlot");

        foreach (GameObject item in tempInventorySlotsList)
        {
            item.SetActive(true);
            TInventorySlot tempSlot = item.GetComponentInChildren<TInventorySlot>();

            if (tempSlot != null)
            {
                AddSlotToInventory(tempSlot);
            }
            TUIManager.SharedInstance.AddInventorySlotUI(item);
        }

        GiveReferenceToThisInventory();
    }

    public void AddSlotToInventory(TInventorySlot inSlotToAdd)
    {
        if (!InventorySlots.Contains(inSlotToAdd))
        {
            InventorySlots.Add(inSlotToAdd);
        }
    }

    //Used to open and close Inventory
    public void ChangeOpenCloseInventoryBool(bool inIsOpen)
    {
        m_isInventoryOpen = inIsOpen;
    }

    public void GiveReferenceToThisInventory()
    {
        TEventManager.TriggerEvent<TInventory>(TEventID.OnInventoryCreated, this);
    }


    #region Collet New Item
    public void CollectItem(TItemQuantity inAddThisItem)
    {
        if (!CheckSimilarItemsAndAddValue(inAddThisItem))
        {
            CheckFreeSlotAndCollect(inAddThisItem);
        }
        m_myPlayer.PlayerCraftComponent.FindAvaibleItems();
    }

    public bool CheckSimilarItemsAndAddValue(TItemQuantity inAddThisItem)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if(InventorySlots[i].ItemInSlot.Item != null)
            {
                if (InventorySlots[i].ItemInSlot.Item == inAddThisItem.Item)
                {
                    InventorySlots[i].ItemInSlot = ChangeSlotValue(InventorySlots[i].ItemInSlot, inAddThisItem.Amount);

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

    public TItemQuantity ChangeSlotValue(TItemQuantity inSlot, int inValue)
    {
        TItemQuantity tempItem = new TItemQuantity(inSlot.Item, inSlot.Amount);
        tempItem.Amount += inValue;
        return tempItem;
    }
    

    public List<TItemQuantity> ItemsInInventory()
    {
        List<TItemQuantity> tempItemsList = new List<TItemQuantity>();

        for(int i = 0; i < InventorySlots.Count; i++)
        {
            if (InventorySlots[i].ItemInSlot != TItemQuantity.Empty)
            {
                tempItemsList.Add(InventorySlots[i].ItemInSlot);
            }
        }
        return tempItemsList;
    }


    public bool CheckSimilarItemsAndRemoveValue(TItemQuantity inAddThisItem)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if (InventorySlots[i].ItemInSlot.Item != null)
            {
                if (InventorySlots[i].ItemInSlot.Item == inAddThisItem.Item)
                {
                    InventorySlots[i].ItemInSlot = ChangeSlotValue(InventorySlots[i].ItemInSlot, -inAddThisItem.Amount);

                    if(InventorySlots[i].ItemInSlot.Amount <= 0)
                    {
                        InventorySlots[i].Clear();
                    }
                    return true;
                }
            }
        }
        return false;
    }
}
