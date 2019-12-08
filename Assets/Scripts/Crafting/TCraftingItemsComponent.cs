using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCraftingItemsComponent : MonoBehaviour
{
    private TPlayerController m_myPlayer;

    //List to store all the player items in inventory.
    public List<TItemQuantity> AllPlayerItems { get; private set; } = new List<TItemQuantity>();

    //List of all craftable item to this player.
    public List<TRecipe> CraftableRecipes { get; private set; } = new List<TRecipe>();

    //Reference to all avaible player crafting slot.
    public List<TCraftingSlot> CraftingSlots { get; private set; } = new List<TCraftingSlot>();


    private void Awake()
    {
        m_myPlayer = GetComponent<TPlayerController>();
    }


    #region Starting crafting slot reference

    /// <summary>
    /// Take the list of craftable slot to the pooler.
    /// And for each of these take his component <param TCraftingSlot="tempSlot"> reference and add it into the List <param CraftingSlots>.
    /// </summary>
    
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

    #endregion

    #region Refresh List of craftable recipes

    /// <summary>
    /// Refresh all the lists and give them to the <param TRecipeContainer>.
    /// It will fill <param CraftableRecipes> after checking wich ones items the player is able to craft.
    /// </summary>

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

    #endregion

    #region Craft Recipe

    /// <summary>
    /// It will remove resources to the player to craft a input recipe.
    /// After that recalls <param FindAvaibleItems>
    /// </summary>

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

    #endregion
}
