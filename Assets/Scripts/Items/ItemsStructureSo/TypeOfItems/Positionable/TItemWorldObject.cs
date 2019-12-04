using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldItem", menuName = "Item/OtherItems/Positionable/OnWorld")]
public class TItemWorldObject : TItemPositionable
{
    public TWorldItem Prefab { get { return m_Prefab; } }

    [SerializeField] private TWorldItem m_Prefab;

    [SerializeField] private TMap m_PlacingLayer;

    public override bool Use(TPlayerController inUser, TPointerData inData)
    {
        if (!inUser.IsInActionRange(inData.GridPosition)) return false;

        Vector3 itemExtents = Prefab.Size.x * TTilemapManager.SharedInstance.CellSize.x / 2 * Vector3.right
                                + Prefab.Size.y * TTilemapManager.SharedInstance.CellSize.y / 2 * Vector3.up;

        Vector3 itemPosition = TTilemapManager.SharedInstance.CellToWorld(inData.GridPosition) + itemExtents;


        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(itemPosition, itemExtents, 0);
        int hitCount = hitColliders.Length;
        TWorldItem hitItem;

        // If there is any object in the space occupied by the Item, return
        for (int i = 0; i < hitCount; i++)
        {
            hitItem = hitColliders[i].GetComponentInParent<TWorldItem>();

            if (hitItem)
            {
                if (hitItem.PlacingLayer == m_PlacingLayer) return false;
            }
            else if (m_PlacingLayer == TMap.Foreground && hitColliders[i].GetComponentInParent<TPlayerController>())
            {
                return false;
            }
        }

        Vector3Int currentCell;

        // Check if each of those tiles can be considered grounded (= has another tile below)
        for (int i = 0; i < Prefab.Size.x; i++)
        {
            currentCell = inData.GridPosition + Vector3Int.right * i;

            // starting from bottom left cell, increase by 1 on the right at each iteration
            // if any cell isn't grounded, return
            if (TTilemapManager.SharedInstance.IsOccupied(currentCell, m_PlacingLayer) || !TTilemapManager.SharedInstance.IsGrounded(currentCell, m_PlacingLayer)) return false;
        }

        TWorldItem item = Instantiate(Prefab, itemPosition, Quaternion.identity);
        item.ReferenceItem = this;
        item.PlacingLayer = m_PlacingLayer;
        item.gameObject.SetActive(true);
        return true;
    }
}
