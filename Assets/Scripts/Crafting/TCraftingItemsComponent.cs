using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCraftingItemsComponent : MonoBehaviour
{
    private TPlayerController m_myPlayer;

    public List<TItemQuantity> AllPlayerItems { get; private set; } = new List<TItemQuantity>();
    public List<TRecipe> CraftableRecipes { get; private set; } = new List<TRecipe>();

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
            TUIManager.SharedInstance.AddCraftingButton(item);
        }
    }


    public void FindAvaibleItems()
    {
        AllPlayerItems.Clear();
        CraftableRecipes.Clear();

        AllPlayerItems = m_myPlayer.PlayerInventoryComponent.ItemsInInventory();
        TRecipeContainer.SharedIstance.CheckCraftableItem(AllPlayerItems, CraftableRecipes);

        FillRecipeInSlots();
    }

    public void FillRecipeInSlots()
    {
        for (int i = 0; i < CraftingSlots.Count; i++)
        {
            if(i > CraftableRecipes.Count - 1)
            {
                CraftingSlots[i].CancelItem();
            }
            else
            {
                CraftingSlots[i].FillSlot(CraftableRecipes[i]);
            }
        }
    }


    public void CraftRecipe(TRecipe inRecipeItem)
    {
        for (int i = 0; i < inRecipeItem.itemsNecessary.Length; i++)
        {
            TItemQuantity tempItemQuantity = new TItemQuantity(inRecipeItem.itemsNecessary[i].Item, inRecipeItem.itemsNecessary[i].Amount);
            m_myPlayer.PlayerInventoryComponent.CheckSimilarItemsAndRemoveValue(tempItemQuantity);
        }
        m_myPlayer.PlayerInventoryComponent.CollectItem(inRecipeItem.itemToObtain);
        FindAvaibleItems();
    }

}
