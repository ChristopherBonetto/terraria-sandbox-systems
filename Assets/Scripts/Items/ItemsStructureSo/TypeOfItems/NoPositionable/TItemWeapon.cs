using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Item/OtherItems/NoPositionable/Weapon")]
public class TItemWeapon : TItem
{
    public LayerMask InteractableLayer;

    [Header("Stats")]

    public int Attack = 1;
    public float TimeToAttack = 1;


    public override bool Use(TPlayerController user, TPointerData inData)
    {
        if (user.CanAttack())
        {
            return true;
        }
        return false;
    }
}
