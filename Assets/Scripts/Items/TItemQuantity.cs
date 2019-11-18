using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItemQuantity
{
    public TItem Item = null;
    public int Amount = 0;

    public TItemQuantity(TItem inItem, int inAmount = 1)
    {
        Item = inItem;
        Amount = inAmount;
    }

    public override string ToString()
    {
        return Item.ItemName + " (" + Amount + ")";
    }
}
