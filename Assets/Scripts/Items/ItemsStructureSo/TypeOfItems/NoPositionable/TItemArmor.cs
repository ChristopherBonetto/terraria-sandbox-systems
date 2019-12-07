using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType
{
    Head,
    Arms,
    Chest,
    Legs
}

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Item/OtherItems/NoPositionable/Armor")]
public class TItemArmor : TItem
{
    public ArmorType ArmorType;
    public AnimatorOverrideController ArmorController;

    public int Defence = 1;
}
