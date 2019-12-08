using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TRecipeContainer : MonoBehaviour
{
    public static TRecipeContainer SharedIstance;

    //Container of all recipes in the game.
    [SerializeField] private TRecipe[] m_recipes;

    private void Awake()
    {
        SharedIstance = this;
    }

    #region Check Craftable Recipes

    /// <summary>
    /// Giving a list of items in input it will fill a fill a list of recipes.
    /// This method check for each recipe contained in <param m_recipes> if the player have the necessary items.
    /// </summary>

    public void CheckCraftableItem(List<TItemQuantity> inListOfItems, List<TRecipe> inListToFill)
    {
        bool canBeCrafted;
        int requiredCount;

        foreach (TRecipe recipe in m_recipes)
        {
            canBeCrafted = true;

            requiredCount = recipe.itemsNecessary.Length;

            for (int i = 0; i < requiredCount; i++)
            {
                if (!inListOfItems.Exists(x => x.Item == recipe.itemsNecessary[i].Item && x.Amount >= recipe.itemsNecessary[i].Amount))
                {
                    canBeCrafted = false;
                    break;
                }
            }

            if (canBeCrafted) inListToFill.Add(recipe);
        }
    }

    #endregion

}
