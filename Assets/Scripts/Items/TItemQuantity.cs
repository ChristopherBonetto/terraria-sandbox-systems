using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct used to contains an <param TItem> and his <param Amount>.
/// </summary>
[System.Serializable]
public struct TItemQuantity
{
    //Used to declare this like empty.
    public static TItemQuantity Empty = new TItemQuantity(null, 0);

    //Reference to the current item contained.
    public TItem Item;

    //Used to increase or decrease the amount of the current item cointained.
    public int Amount;

    //Used to initializate this Struct.
    public TItemQuantity(TItem inItem, int inAmount = 1)
    {
        this.Item = inItem;
        this.Amount = inAmount;
    }


    public override string ToString()
    {
        return Item.ItemName + " (" + Amount + ")";
    }

    #region Comparison between items

    /// <summary>
    /// Used to compare two Items and their amounts.
    /// </summary>

    public static bool operator ==(TItemQuantity q1, TItemQuantity q2)
    {
        return q1.Item == q2.Item && q1.Amount == q2.Amount;
    }

    public static bool operator !=(TItemQuantity q1, TItemQuantity q2)
    {
        return q1.Item != q2.Item && q1.Amount != q2.Amount;
    }

    #endregion
}
