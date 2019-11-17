using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewItem", fileName = "Item")]
public class TItemScriptable : ScriptableObject
{
    public new string ItemName = "";
    
    public int Attack = 1;
    public int Defence = 1;
    public float TimeToAttack = 1;
    
    [Space]
    public Sprite ItemSprite;
    
}


[System.Serializable]
public struct TItemQuantity
{

    public TItemScriptable Item;
    public int Amount;

    public TItemQuantity(TItemScriptable inItem, int inAmount = 1)
    {
        Item = inItem;
        Amount = inAmount;
    }
    
    public override string ToString()
    {
        return Item.ItemName + " (" + Amount + ")";
    }
}
