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
        if (!inUser.IsInActionRange(inData.GridPosition))
            return false;

        TWorldItem item = Instantiate(Prefab);

        Vector3 boundsExtents = item.ColliderComponent.bounds.extents;

        item.gameObject.SetActive(false);

        Vector3 itemPosition = TTilemapManager.SharedInstance.CellToWorld(inData.GridPosition) + boundsExtents;

        Collider2D cool = Physics2D.OverlapBox(itemPosition, boundsExtents, 0);

        // If there is any object in the space occupied by the Item, return
        if (Physics2D.OverlapBox(itemPosition, boundsExtents, 0))
        {
            Destroy(item.gameObject);
            return false;
        }

        // Calculate the number of cells occupied by the Item that should be checked for grounding:
        // number of tiles = [Item width] / [cell width] (rounded down)
        int cellsNum = Mathf.FloorToInt(2 * boundsExtents.x / TTilemapManager.SharedInstance.CellSize.x);

        Vector3Int currentCell;

        // Check if each of those tiles can be considered grounded (= has another tile below)
        for (int i = 0; i < cellsNum; i++)
        {
            currentCell = inData.GridPosition + Vector3Int.right * i;

            // starting from bottom left cell, increase by 1 on the right at each iteration
            // if any cell isn't grounded, return
            if (TTilemapManager.SharedInstance.IsOccupied(currentCell, TMap.Foreground) || !TTilemapManager.SharedInstance.IsGrounded(currentCell))
            {
                Destroy(item.gameObject);
                return false;
            }
        }

        item.TransformComponent.position = itemPosition;
        item.ReferenceItem = this;
        item.gameObject.SetActive(true);
        return true;
    }
}
