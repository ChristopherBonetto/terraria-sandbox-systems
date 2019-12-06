using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TItemPositionable : TItem, IUsable
{
    public virtual bool Use(TPlayerController inUser, TPointerData inData)
    {
        return false;
    }
}
