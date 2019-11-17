using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon : IAttack
{
    TItemWeapon WeaponEquipped { get; }
}
