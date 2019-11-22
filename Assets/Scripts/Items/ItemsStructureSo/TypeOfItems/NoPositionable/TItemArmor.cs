using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Item/OtherItems/NoPositionable/Armor")]
public class TItemArmor : TItem
{
    public int Defence = 1;

    public override bool Use(TPlayerController user, TPointerData inData)
    {
        throw new System.NotImplementedException();
    }
}
