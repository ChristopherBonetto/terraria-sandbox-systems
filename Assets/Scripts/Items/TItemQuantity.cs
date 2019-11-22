using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TItemQuantity
{
    public static TItemQuantity Empty = new TItemQuantity(null, 0);

    public TItem Item;
    public int Amount;

    public TItemQuantity(TItem inItem, int inAmount = 1)
    {
        this.Item = inItem;
        this.Amount = inAmount;
    }

    public override string ToString()
    {
        return Item.ItemName + " (" + Amount + ")";
    }

    public static bool operator ==(TItemQuantity q1, TItemQuantity q2)
    {
        return q1.Item == q2.Item && q1.Amount == q2.Amount;
    }

    public static bool operator !=(TItemQuantity q1, TItemQuantity q2)
    {
        return q1.Item != q2.Item && q1.Amount != q2.Amount;
    }
}
