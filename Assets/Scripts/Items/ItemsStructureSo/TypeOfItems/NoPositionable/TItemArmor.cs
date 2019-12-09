using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType
{
    Head,
    Chest,
    Legs
}

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Item/OtherItems/NoPositionable/Armor")]
public class TItemArmor : TItem
{
    [Header("Assign Armor Type")]
    public ArmorType ArmorType;

    [Header("Assign a controller to override to player")]
    public AnimatorOverrideController ArmorAnim;

    [Header("Armor statistics to add")]
    public TStatistics Statistics;

    public override List<string> TakeAllInfos()
    {
        m_infos = new List<string>();
        m_infos.Add("Item name : ");
        m_infos.Add(ItemName);
        m_infos.Add("");

        m_infos.Add("Defense :" + Statistics.Defense);
        m_infos.Add("");

        m_infos.Add("Description :");
        m_infos.Add(m_descriptionField);

        return m_infos;
    }
}
