using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItemWeapon : TItem
{
    public virtual void OnUse()
    {
        Debug.Log("Attacking...");
    }
}
