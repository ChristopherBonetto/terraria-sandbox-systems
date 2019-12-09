using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileItem", menuName = "Item/OtherItems/Positionable/OnTilemap")]
public class TItemTileObject : TItemPositionable
{

    #region Public properties

    /// <summary>
    /// Reference to the Tile's Scriptable Object.
    /// </summary>
    public TDestructibleTile Tile { get { return m_Tile; } }

    /// <summary>
    /// Tilemap on which the Tile should be placed:
    /// </summary>
    public TMap TargetTilemap { get { return m_TargetTilemap; } }

    #endregion

    #region Serialized variables

    [Header("Tile data")]

    /// <summary>
    /// Reference to the Tile's Scriptable Object.
    /// </summary>
    [SerializeField] private TDestructibleTile m_Tile;

    /// <summary>
    /// Tilemap on which the Tile should be placed:
    /// </summary>
    [SerializeField] private TMap m_TargetTilemap;

    #endregion


    public override bool Use(TPlayerController inUser, TPointerData inData)
    {
        // The following conditions should be met:
        // 1. Target position must be in placing range.
        // 2. Target cell must not be occupied already.
        // 3. There should be at least one neighbor on the Tilemaps (four neighbours on target map and same position on the other map)
        // 4. The corresponding cell should not be occupied by any object on the same layer.
        if (inUser.IsInActionRange(inData.GridPosition)
            && !TTilemapManager.SharedInstance.IsOccupied(inData.GridPosition, m_TargetTilemap)
            && TTilemapManager.SharedInstance.CheckForNeighbors(inData.GridPosition, m_TargetTilemap))
        {
            // Check for objects occupying cell space
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(TTilemapManager.SharedInstance.CellToWorldCenter(inData.GridPosition), TTilemapManager.SharedInstance.CellSize / 2, 0);

            int hitCount = hitColliders.Length;

            if (hitCount > 0)
            {
                TWorldItem hitItem;

                for (int i = 0; i < hitCount; i++)
                {
                    hitItem = hitColliders[i].GetComponentInParent<TWorldItem>();

                    // If an Item was hit and it's on the same layer, stop
                    if (hitItem)
                    {
                        if (hitItem.PlacingLayer == m_TargetTilemap) return false;
                    }

                    // Otherwise, it the target is Foreground, check if any entity is currently occupying the cell
                    else if (m_TargetTilemap == TMap.Foreground && hitColliders[i].GetComponentInParent<BaseEntity>()) return false;
                }
            }

            // If all clear, set tile
            TTilemapManager.SharedInstance.SetTile(inData.GridPosition, m_Tile, m_TargetTilemap);

            return true;
        }

        else return false;
    }
}
