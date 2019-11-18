using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldItem", menuName = "Item/OtherItems/Positionable/OnWorld")]
public class TItemWorldObject : TItemPositionable
{
    public TWorldItem Prefab { get { return m_Prefab; } }

    [SerializeField] private TWorldItem m_Prefab;
}
