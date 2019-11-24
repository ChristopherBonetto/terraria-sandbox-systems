using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct RecipeInfo
{
    public TItemQuantity[] itemsNecessary;
    [Space]
    public TItemQuantity itemToObtain;
}

public class TRecipeContainer : MonoBehaviour
{
    public static TRecipeContainer SharedIstance;

    [SerializeField] private RecipeInfo[] m_recipes;

    public Dictionary<TItemQuantity, RecipeInfo> RecipeDictionary { get; private set; }

    private void Awake()
    {
        SharedIstance = this;

        RecipeDictionary = new Dictionary<TItemQuantity, RecipeInfo>();

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

    private void Start()
    {
        Debug.Log(RecipeDictionary.Count);
    }


    public void CheckCraftableItem(List<TItemQuantity> inListOfItems, List<TItemQuantity> inListToFill)
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
                    inListToFill.Add(item);
                }
            }
        }
    }
}
