using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileItem", menuName = "Item/OtherItems/Positionable/OnTilemap")]
public class TItemTileObject : TItemPositionable
{
    public TDestructibleTile Tile { get { return m_Tile; } }
    public TMap TargetTilemap { get { return m_TargetTilemap; } }

    [SerializeField] private TDestructibleTile m_Tile;
    [SerializeField] private TMap m_TargetTilemap;
}
