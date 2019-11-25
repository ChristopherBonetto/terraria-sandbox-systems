using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldItem", menuName = "Item/OtherItems/Positionable/OnWorld")]
public class TItemWorldObject : TItemPositionable
{
    public TWorldItem Prefab { get { return m_Prefab; } }

    [SerializeField] private TWorldItem m_Prefab;

    public override bool Use(TPlayerController inUser, TPointerData inData)
    {
        if (!inUser.IsInActionRange(inData.GridPosition)) return false;

        Vector3 itemExtents = Prefab.Size.x * TTilemapManager.SharedInstance.CellSize.x / 2 * Vector3.right
                                + Prefab.Size.y * TTilemapManager.SharedInstance.CellSize.y / 2 * Vector3.up;

        Vector3 itemPosition = TTilemapManager.SharedInstance.CellToWorld(inData.GridPosition) + itemExtents;

        // If there is any object in the space occupied by the Item, return
        if (Physics2D.OverlapBox(itemPosition, itemExtents, 0)) return false;

        Vector3Int currentCell;

        // Check if each of those tiles can be considered grounded (= has another tile below)
        for (int i = 0; i < Prefab.Size.x; i++)
        {
            currentCell = inData.GridPosition + Vector3Int.right * i;

            // starting from bottom left cell, increase by 1 on the right at each iteration
            // if any cell isn't grounded, return
            if (TTilemapManager.SharedInstance.IsOccupied(currentCell, TMap.Foreground) || !TTilemapManager.SharedInstance.IsGrounded(currentCell)) return false;
        }

        TWorldItem item = Instantiate(Prefab, itemPosition, Quaternion.identity);
        item.ReferenceItem = this;
        item.gameObject.SetActive(true);
        return true;
    }
}
