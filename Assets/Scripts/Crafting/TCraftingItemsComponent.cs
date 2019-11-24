using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCraftingItemsComponent : MonoBehaviour
{
    private TPlayerController m_myPlayer;

    private List<TItemQuantity> AllPlayerItems;
    public List<TItemQuantity> CraftableItems;

    public List<TCraftingSlot> CraftingSlots = new List<TCraftingSlot>();


    private void Awake()
    {
        m_myPlayer = GetComponent<TPlayerController>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (m_myPlayer.PlayerInventory.InventoryIsOpen)
        {
            FindAvaibleItems();

            FillSlots();
        }
    }

    public void CraftingButtonsReference()
    {
        List<GameObject> CraftableSlotsObect = new List<GameObject>();
        CraftableSlotsObect = ObjectPooler.SharedInstance.ReturnListFromDictionary("CraftingButton");

        foreach (GameObject item in CraftableSlotsObect)
        {
            TCraftingSlot tempSlot = item.GetComponent<TCraftingSlot>();

            if(tempSlot != null)
            {
                if (!CraftingSlots.Contains(tempSlot))
                {
                    tempSlot.m_myPlayer = m_myPlayer;
                    CraftingSlots.Add(tempSlot);
                }
            }
            UIManager.SharedInstance.AddCraftingButton(item);
        }
    }




    public void FindAvaibleItems()
    {
        CraftableItems = new List<TItemQuantity>();
        AllPlayerItems = new List<TItemQuantity>();

        AllPlayerItems = m_myPlayer.PlayerInventory.ItemsInInventory();
        TRecipeContainer.SharedIstance.CheckCraftableItem(AllPlayerItems, CraftableItems);
    }

    public void FillSlots()
    {
        for(int i = 0; i < CraftableItems.Count; i++)
        {
            CraftingSlots[i].FillSlot(CraftableItems[i]);
        }
    }
    
}
