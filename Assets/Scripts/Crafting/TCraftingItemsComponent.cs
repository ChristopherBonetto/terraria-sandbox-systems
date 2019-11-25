using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCraftingItemsComponent : MonoBehaviour
{
    private TPlayerController m_myPlayer;

    public List<TItemQuantity> AllPlayerItems = new List<TItemQuantity>();
    public List<TRecipeInfo> CraftableItems = new List<TRecipeInfo>();

    public List<TCraftingSlot> CraftingSlots { get; private set; } = new List<TCraftingSlot>();


    private void Awake()
    {
        m_myPlayer = GetComponent<TPlayerController>();
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
                    tempSlot.FillPlayerController(m_myPlayer);
                    CraftingSlots.Add(tempSlot);
                }
            }
            UIManager.SharedInstance.AddCraftingButton(item);
        }
    }


    public void FindAvaibleItems()
    {
        AllPlayerItems.Clear();
        CraftableItems.Clear();

        AllPlayerItems = m_myPlayer.PlayerInventory.ItemsInInventory();
        TRecipeContainer.SharedIstance.CheckCraftableItem(AllPlayerItems, CraftableItems);

        FillRecipeInSlots();
    }

    public void FillRecipeInSlots()
    {
        for (int i = 0; i < CraftingSlots.Count; i++)
        {
            if(i > CraftableItems.Count - 1)
            {
                CraftingSlots[i].CancelItem();
            }
            else
            {
                CraftingSlots[i].FillSlot(CraftableItems[i]);
            }
        }
    }


    public void CraftRecipe(TRecipeInfo inRecipeItem)
    {
        for (int i = 0; i < inRecipeItem.itemsNecessary.Length; i++)
        {
            TItemQuantity tempItemQuantity = new TItemQuantity(inRecipeItem.itemsNecessary[i].Item, inRecipeItem.itemsNecessary[i].Amount);
            m_myPlayer.PlayerInventory.CheckSimilarItemsAndRemoveValue(tempItemQuantity);
        }
        m_myPlayer.PlayerInventory.CollectItem(inRecipeItem.itemToObtain);
        FindAvaibleItems();
    }

}
