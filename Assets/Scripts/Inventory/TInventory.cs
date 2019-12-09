using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInventory : MonoBehaviour
{
    private TPlayerController m_myPlayer;

    //List or items that the player will equip to the start of the game.
    [SerializeField] private List<TItemQuantity> StartingItems = new List<TItemQuantity>();

    //List that it contains the reference for each TInventorySlot.
    public List<TInventorySlot> InventorySlots { get; private set; } = new List<TInventorySlot>();

    public bool m_isInventoryOpen { get; private set; }


    #region Events

    private void OnEnable()
    {
        TEventManager.SubscribeTo<bool>(TEventID.OnInventoryOpen, ChangeOpenCloseInventoryBool);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<bool>(TEventID.OnInventoryOpen, ChangeOpenCloseInventoryBool);
    }

    #endregion


    private void Awake()
    {
        m_myPlayer = gameObject.GetComponent<TPlayerController>();
    }

    #region Collect starting items

    private void Start()
    {
        StartCoroutine(CollectStartingItems());
    }

    /// <summary>
    /// Coroutine that it wait the end of the current frame to collect all the starting items.
    /// </summary>
    /// <returns></returns>
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

    #endregion

    #region Create Inventory

    /// <summary>
    /// This method copy a list by tag from the item pooler.
    /// For each element in list take his <param TInventorySlot> component and add it to <param InventorySlots> list.
    /// After that trigger an event to tell to all subscribed class that it is the main inventory class.
    /// </summary>
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


    public void GiveReferenceToThisInventory()
    {
        TEventManager.TriggerEvent<TInventory>(TEventID.OnInventoryCreated, this);
    }

    #endregion

    #region Collet New Item

    /// <summary>
    /// This method does two checks.
    /// One check if the inventory contains an item equal to the input item. if return true increase the amount of that item.
    /// If the first check return false so check the first empty slot and fill it with the input item.
    /// </summary>
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

    #region Decrease amount to an slotted item.

    /// <summary>
    /// Used to check if exist in list the input's item. If return true decreases his amount.
    /// </summary>
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

    #endregion

    #region Methods to take/change inventory infos.

    //Triggered from event change the bool of this inventory.
    public void ChangeOpenCloseInventoryBool(bool inIsOpen)
    {
        m_isInventoryOpen = inIsOpen;
    }

    /// <summary>
    /// Used to change the amount of an item.
    /// </summary>
    public TItemQuantity ChangeSlotValue(TItemQuantity inSlot, int inValue)
    {
        TItemQuantity tempItem = new TItemQuantity(inSlot.Item, inSlot.Amount);
        tempItem.Amount += inValue;
        return tempItem;
    }

    /// <summary>
    /// Returns a list of all items in inventory.
    /// </summary>
    public List<TItemQuantity> ItemsInInventory()
    {
        List<TItemQuantity> tempItemsList = new List<TItemQuantity>();

        for (int i = 0; i < InventorySlots.Count; i++)
        {
            if (InventorySlots[i].ItemInSlot != TItemQuantity.Empty)
            {
                tempItemsList.Add(InventorySlots[i].ItemInSlot);
            }
        }
        return tempItemsList;
    }

    #endregion
}
