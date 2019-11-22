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

    public override bool Use(TPlayerController inUser, TPointerData inData)
    {
        // The following conditions should be met:
        // 1. Target position must be in placing range.
        // 2. There should be at least one neighbor on the Tilemaps (four neighbours on target map and same position on the other map)
        // 3. The corresponding cell should not be occupied by any object.
        if (inUser.IsInActionRange(inData.GridPosition)
            && !TTilemapManager.SharedInstance.IsOccupied(inData.GridPosition, m_TargetTilemap)
            && TTilemapManager.SharedInstance.CheckForNeighbors(inData.GridPosition, m_TargetTilemap)
            && !Physics2D.OverlapBox(TTilemapManager.SharedInstance.CellToWorldCenter(inData.GridPosition), TTilemapManager.SharedInstance.CellSize / 2, 0))
        {
            // Set tile
            TTilemapManager.SharedInstance.SetTile(inData.GridPosition, m_Tile, m_TargetTilemap);
            return true;
        }

        else return false;
    }
}
