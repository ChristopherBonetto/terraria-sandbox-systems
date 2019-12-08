using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
public class TRecipe : ScriptableObject
{
    //Array of necessary items.
    public TItemQuantity[] itemsNecessary;

    [Space]
    //Item obtained crafting this recipe
    public TItemQuantity itemToObtain;


    public TRecipe(TItemQuantity[] inItemsNecessary, TItemQuantity inItemObtained)
    {
        this.itemsNecessary = inItemsNecessary;
        this.itemToObtain = inItemObtained;
    }
}
