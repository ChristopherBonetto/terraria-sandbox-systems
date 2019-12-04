using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public struct TRecipeInfo
//{
//    public static TRecipeInfo Empty = new TRecipeInfo(null, TItemQuantity.Empty);

//    public TItemQuantity[] itemsNecessary;
//    [Space]
//    public TItemQuantity itemToObtain;

//    public TRecipeInfo(TItemQuantity[] inItemsNecessary, TItemQuantity inItemObtained)
//    {
//        this.itemsNecessary = inItemsNecessary;
//        this.itemToObtain = inItemObtained;
//    }
//}

public class TRecipeContainer : MonoBehaviour
{
    public static TRecipeContainer SharedIstance;

    [SerializeField] private TRecipe[] m_recipes;

    public Dictionary<TItemQuantity, TRecipe> RecipeDictionary { get; private set; }

    private void Awake()
    {
        SharedIstance = this;

        CreateDictionaryOfRecipe();
    }

    public void CreateDictionaryOfRecipe()
    {
        RecipeDictionary = new Dictionary<TItemQuantity, TRecipe>();

        for (int i = 0; i < m_recipes.Length; i++)
        {
            if (!RecipeDictionary.ContainsKey(m_recipes[i].itemToObtain))
            {
                RecipeDictionary.Add(m_recipes[i].itemToObtain, m_recipes[i]);
            }
            else
            {
                Debug.Log(m_recipes[i].itemToObtain + " can't be added because there is another key with same value");
            }
        }
    }
    

    public void CheckCraftableItem(List<TItemQuantity> inListOfItems, List<TRecipe> inListToFill)
    {    
        foreach (TItemQuantity item in RecipeDictionary.Keys)
        {
            int itemNecessary = 0;

            for (int i = 0; i < RecipeDictionary[item].itemsNecessary.Length; i++)
            {
                if (itemNecessary < RecipeDictionary[item].itemsNecessary.Length)
                {
                    TItemQuantity tempItemQuantity = new TItemQuantity(RecipeDictionary[item].itemsNecessary[i].Item, RecipeDictionary[item].itemsNecessary[i].Amount);

                    if (inListOfItems.Exists(x => x.Item.ItemName.Contains(tempItemQuantity.Item.ItemName)))
                    {
                        TItemQuantity tempItem = inListOfItems.Find(x => x.Item.ItemName.Contains(tempItemQuantity.Item.ItemName));

                        if (tempItem.Amount >= tempItemQuantity.Amount)
                        {
                            itemNecessary++;
                        }
                        else
                        {
                            Debug.Log("u need materials");
                            break;
                        }
                    }
                }
                if (itemNecessary == RecipeDictionary[item].itemsNecessary.Length)
                {
                    inListToFill.Add(RecipeDictionary[item]);
                }
            }
        }
    }
    
}
