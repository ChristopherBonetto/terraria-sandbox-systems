using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TPlayerController))]
public class TItemPlacer : MonoBehaviour
{
    #region Public properties

    /// <summary>
    /// Cached reference to the attached Transform component.
    /// </summary>
    public Transform TransformComponent { get; private set; }

    #endregion

    #region Serialized variables

    [Header("Constraints")]
    [Tooltip("Maximum distance (in grid units) at which an Item may be placed.")]
    [SerializeField] private int m_MaxDistance;

    [Header("Testing")]
    [SerializeField] private TDestructibleTile m_TestTile;
    [SerializeField] private TWorldItem m_TestWorldItem;

    #endregion

    #region Private variables

    // Current Tile being placed
    private TDestructibleTile m_CurrentTile;
    private TMap m_TargetTilemap;

    // Current World Item being placed
    private TWorldItem m_CurrentWorldItem;

    // Bounds of the Current World Item's collider
    private Vector3 m_CurrentBoundsExtents;

    private TInventorySlot m_ReferencedSlot;

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        // Cache transform
        TransformComponent = transform;
    }

    private void Start()
    {
        //TItemHandler.Instance.OnSelectedSlotEventAction += LoadItem;
    }

    private void OnDestroy()
    {
        //TItemHandler.Instance.OnSelectedSlotEventAction -= LoadItem;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Loads an Items on the placer, activating it.
    /// </summary>
    /// <param name="inSlot"></param>
    public void LoadItem(TInventorySlot inSlot)
    {
        if (inSlot.ItemInSlot.Item is TItemTileObject)
        {
            LoadTile((TItemTileObject)inSlot.ItemInSlot.Item);
            m_ReferencedSlot = inSlot;
        }

        else if (inSlot.ItemInSlot.Item is TItemWorldObject)
        {
            LoadWorldItem((TItemWorldObject)inSlot.ItemInSlot.Item);
            m_ReferencedSlot = inSlot;
        }
    }

    #region Test methods

    /// <summary>
    /// Loads a specific Tile to be placed.
    /// </summary>
    /// <param name="inItem"></param>
    public void LoadTile(TItemTileObject inItem)
    {
        // Set current tile
        m_CurrentTile = inItem.Tile;
        m_TargetTilemap = inItem.TargetTilemap;

        // Subscribe to click event
        TInputManager.SharedInstance.OnLeftClickDown += PlaceTile;
    }

    /// <summary>
    /// Loads a specific World Item to be placed.
    /// </summary>
    /// <param name="inWorldItemPrefab"></param>
    public void LoadWorldItem(TItemWorldObject inItem)
    {
        InstantiateWorldItem(inItem.Prefab);

        // Subscribe to input events
        TInputManager.SharedInstance.OnLeftClickDown += PlaceWorldItem;
        TInputManager.SharedInstance.OnPointerMovedOnGrid += FollowPointer;
    }

    #endregion

    #endregion

    #region Private methods

    /// <summary>
    /// Places current Tile at the position specified by Pointer Data.
    /// </summary>
    /// <param name="inData"></param>
    private void PlaceTile(TPointerData inData)
    {
        // The following conditions should be met:
        // 1. Target position must be in placing range.
        // 2. There should be at least one neighbor on the Tilemaps (four neighbours on target map and same position on the other map)
        // 3. The corresponding cell should not be occupied by any object.
        if (IsInRange(inData.GridPosition)
            && TTilemapManager.SharedInstance.CheckForNeighbors(inData.GridPosition, m_TargetTilemap)
            && !Physics2D.OverlapBox(TTilemapManager.SharedInstance.CellToWorldCenter(inData.GridPosition), TTilemapManager.SharedInstance.CellSize / 2, 0))
        {
            // Set tile and unsubscribe
            TTilemapManager.SharedInstance.SetTile(inData.GridPosition, m_CurrentTile, m_TargetTilemap);

            //m_ReferencedSlot.DepleteAmount(1);
           
            if (m_ReferencedSlot.ItemInSlot.Item == null)
                TInputManager.SharedInstance.OnLeftClickDown -= PlaceTile;
        }
    }

    /// <summary>
    /// Places current World Item in such a way that the position specified by provided Pointer Data corresponds to the Item's bottom left corner.
    /// </summary>
    /// <param name="inData"></param>
    private void PlaceWorldItem(TPointerData inData)
    {
        // If there is any object in the space occupied by the Item, return
        if (Physics2D.OverlapBox(m_CurrentWorldItem.TransformComponent.position, m_CurrentBoundsExtents, 0))
            return;

        // Get the cell corresponding to the bottom-left corner of the World Item
        Vector3Int bottomLeftCell = TTilemapManager.SharedInstance.WorldToGridPosition(m_CurrentWorldItem.TransformComponent.position - m_CurrentBoundsExtents);

        // Calculate the number of cells occupied by the Item that should be checked for grounding:
        // number of tiles = [Item width] / [cell width] (rounded down)
        int cellsNum = Mathf.FloorToInt(2 * m_CurrentBoundsExtents.x / TTilemapManager.SharedInstance.CellSize.x);

        // Check if each of those tiles can be considered grounded (= has another tile below)
        for (int i = 0; i < cellsNum; i++)
        {
            // starting from bottom left cell, increase by 1 on the right at each iteration
            // if any cell isn't grounded, return
            if (!TTilemapManager.SharedInstance.CheckForGrounding(bottomLeftCell + GridUtility.Directions[(int)Direction.Right] * i))
                return;
        }

        // If everything was fine, restore original alpha
        Color col = m_CurrentWorldItem.RendererComponent.material.color;
        col.a = 1.0f;
        m_CurrentWorldItem.RendererComponent.material.color = col;

        // Reenable collider
        m_CurrentWorldItem.ColliderComponent.enabled = true;

        //m_ReferencedSlot.DepleteAmount(1);

        if (m_ReferencedSlot.ItemInSlot.Item == null)
        {
            // Unsubscribe from input events
            TInputManager.SharedInstance.OnLeftClickDown -= PlaceWorldItem;
            TInputManager.SharedInstance.OnPointerMovedOnGrid -= FollowPointer;
        }
        else
        {
            InstantiateWorldItem(((TItemWorldObject)m_ReferencedSlot.ItemInSlot.Item).Prefab);
        }

    }

    /// <summary>
    /// Places current World Item at the pointer's position.
    /// </summary>
    /// <param name="inData"></param>
    private void FollowPointer(TPointerData inData)
    {
        // If the pointer is in range, update position
        // Else hide the Item and remove ability to place it on click (till it's in range again)

        if (IsInRange(inData.GridPosition))
        {
            // Update position
            m_CurrentWorldItem.TransformComponent.position = TTilemapManager.SharedInstance.CellToWorld(inData.GridPosition) + m_CurrentBoundsExtents;

            // If the Item was hidden, show it again
            if (!m_CurrentWorldItem.gameObject.activeInHierarchy)
            {
                TInputManager.SharedInstance.OnLeftClickDown += PlaceWorldItem;
                m_CurrentWorldItem.gameObject.SetActive(true);
            }
        }
        else
        {
            // If the Item isn't hidden, hide it
            if (m_CurrentWorldItem.gameObject.activeInHierarchy)
            {
                TInputManager.SharedInstance.OnLeftClickDown -= PlaceWorldItem;
                m_CurrentWorldItem.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Check if the specified cell is in placing range.
    /// </summary>
    /// <param name="inCell"></param>
    /// <returns>True if the cell is in range.</returns>
    private bool IsInRange(Vector3Int inCell)
    {
        // In range if: | [cell to check] - [placer cell] | <= MaxDistance (rounded)
        return Mathf.CeilToInt(Vector3Int.Distance(TTilemapManager.SharedInstance.WorldToGridPosition(TransformComponent.position), inCell)) <= m_MaxDistance;
    }

    private void InstantiateWorldItem(TWorldItem inWorldItem)
    {
        // Instantiate copy
        m_CurrentWorldItem = Instantiate(inWorldItem, TTilemapManager.SharedInstance.TransformComponent);

        // Set Renderer's color as half-transparent
        Color col = m_CurrentWorldItem.RendererComponent.material.color;
        col.a = 0.5f;
        m_CurrentWorldItem.RendererComponent.material.color = col;

        // Save bounds extents before disabling the collider
        m_CurrentBoundsExtents = m_CurrentWorldItem.ColliderComponent.bounds.extents;
        m_CurrentWorldItem.ColliderComponent.enabled = false;
    }

    #endregion
}
