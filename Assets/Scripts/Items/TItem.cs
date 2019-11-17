using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItem : MonoBehaviour, IUsable
{
    public TItemQuantity StatsOfThisItem;

    public virtual void OnUse()
    {
    }
}
