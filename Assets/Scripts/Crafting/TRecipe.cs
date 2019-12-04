using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
public class TRecipe : ScriptableObject
{
    public TItemQuantity[] itemsNecessary;
    [Space]
    public TItemQuantity itemToObtain;

    public TRecipe(TItemQuantity[] inItemsNecessary, TItemQuantity inItemObtained)
    {
        this.itemsNecessary = inItemsNecessary;
        this.itemToObtain = inItemObtained;
    }
}
