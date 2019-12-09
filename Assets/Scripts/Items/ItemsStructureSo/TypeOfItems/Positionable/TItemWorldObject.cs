using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldItem", menuName = "Item/OtherItems/Positionable/OnWorld")]
public class TItemWorldObject : TItemPositionable
{
    #region Public properties

    /// <summary>
    /// Prefab of the placed Item.
    /// </summary>
    public TWorldItem Prefab { get { return m_Prefab; } }

    /// <summary>
    /// Size of the placed Item (in cells)
    /// </summary>
    public Vector2Int Size { get { return m_Size; } }

    /// <summary>
    /// Layer on which the Item should be placed.
    /// </summary>
    public TMap PlacingLayer { get { return m_PlacingLayer; } }

    #endregion

    #region Serialized variables

    [Header("Placing data")]

    [SerializeField] private TWorldItem m_Prefab;

    [SerializeField] private Vector2Int m_Size;

    [SerializeField] private TMap m_PlacingLayer;

    #endregion


    public override bool Use(TPlayerController inUser, TPointerData inData)
    {
        // 1. Check if the clicked point is in user's action range
        if (!inUser.IsInActionRange(inData.GridPosition)) return false;

        // Calculate item extents and position

        Vector3 itemExtents = m_Size.x * TTilemapManager.SharedInstance.CellSize.x / 2 * Vector3.right
                                + m_Size.y * TTilemapManager.SharedInstance.CellSize.y / 2 * Vector3.up;

        Vector3 itemPosition = TTilemapManager.SharedInstance.CellToWorld(inData.GridPosition);


        // 2. Check if the placing space is occupied by anything
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(itemPosition + itemExtents, itemExtents, 0);
        int hitCount = hitColliders.Length;

        if (hitCount > 0)
        {
            TWorldItem hitItem;

            // If there is any object in the space occupied by the Item, return
            for (int i = 0; i < hitCount; i++)
            {
                hitItem = hitColliders[i].GetComponentInParent<TWorldItem>();

                if (hitItem)
                {
                    // If the space is occupied by another Item on the same layer, stop
                    if (hitItem.PlacingLayer == m_PlacingLayer) return false;
                }

                // Otherwise if the target layer is Foreground, check if any entity is occupying the space
                else if (m_PlacingLayer == TMap.Foreground && hitColliders[i].GetComponentInParent<BaseEntity>())
                {
                    return false;
                }
            }
        }

        Vector3Int currentCell;

        // 3. Check if each of the cells occupied by the Item can be considered grounded (= has another tile below) and is not occupied
        for (int i = 0; i < m_Size.x; i++)
        {
            currentCell = inData.GridPosition + Vector3Int.right * i;

            // starting from bottom left cell, increase by 1 on the right at each iteration
            // if any cell isn't grounded, return
            if (TTilemapManager.SharedInstance.IsOccupied(currentCell, m_PlacingLayer) || !TTilemapManager.SharedInstance.IsGrounded(currentCell, m_PlacingLayer)) return false;
        }


        // If all clear, instantiate and init the Item

        TWorldItem item = Instantiate(Prefab, itemPosition, Quaternion.identity);
        item.ReferenceItem = this;
        item.PlacingLayer = m_PlacingLayer;
        item.gameObject.SetActive(true);
        return true;
    }
}
