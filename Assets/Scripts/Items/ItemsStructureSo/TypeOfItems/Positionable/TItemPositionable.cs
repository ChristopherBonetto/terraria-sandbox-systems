using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TItemPositionable : TItem, IUsable
{
    public bool DepleteOnUse { get { return m_DepleteOnUse; } }

    //Boolean used to check if it can decreases his amount.
    [SerializeField] private bool m_DepleteOnUse;

    public virtual bool Use(TPlayerController inUser, TPointerData inData)
    {
        return false;
    }
}
