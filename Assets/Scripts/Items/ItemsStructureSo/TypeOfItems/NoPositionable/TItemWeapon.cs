using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Item/OtherItems/NoPositionable/Weapon")]
public class TItemWeapon : TItemNoPositionable
{
    public int Attack = 1;

    public float TimeToAttack = 1;
}
