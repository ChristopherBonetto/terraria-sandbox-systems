using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemInfo
{
    public TItem item;
    public int amount;
}

[System.Serializable]
public struct RecipeInfo
{
    public ItemInfo[] itemsNecessary;
    [Space]
    public ItemInfo itemToObtain;
}

public class TRecipeContainer : MonoBehaviour
{
    public static TRecipeContainer SharedIstance;

    [SerializeField] private RecipeInfo[] m_recipes;

    public Dictionary<TItem, RecipeInfo> RecipeDictionary { get; private set; }

    private void Awake()
    {
        SharedIstance = this;

        RecipeDictionary = new Dictionary<TItem, RecipeInfo>();

        for (int i = 0; i < m_recipes.Length; i++)
        {
            if (!RecipeDictionary.ContainsKey(m_recipes[i].itemToObtain.item))
            {
                RecipeDictionary.Add(m_recipes[i].itemToObtain.item, m_recipes[i]);
            }
            else
            {
                Debug.Log(m_recipes[i].itemToObtain + " can't be added because there is another key with same value");
            }
        }
    }
    

    public void CheckCraftableItem(TItem inItemOne)
    {
        foreach (TItem item in RecipeDictionary.Keys)
        {
            for (int i = 0; i < RecipeDictionary[item].itemsNecessary.Length; i++)
            {
                if (inItemOne == RecipeDictionary[item].itemsNecessary[i].item)
                {
                    Debug.Log(RecipeDictionary[item].itemsNecessary[i].item.ItemName);
                }
            }
        }
        
    }
}
