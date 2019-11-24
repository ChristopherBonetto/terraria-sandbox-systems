using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum TMap
{
    Background,
    Foreground,
    Placement
}

public class TTilemapManager : MonoBehaviour
{
    #region Static properties

    /// <summary>
    /// Singleton instance reference.
    /// </summary>
    public static TTilemapManager SharedInstance { get; private set; }

    #endregion

    #region Public properties

    /// <summary>
    /// Grid cell size.
    /// </summary>
    public Vector3 CellSize { get { return m_ForegroundMap.cellSize; } }

    /// <summary>
    /// Cached reference to the attached Transform component.
    /// </summary>
    public Transform TransformComponent { get; private set; }

    #endregion

    #region Serialized variables

    [SerializeField] private Tilemap m_ForegroundMap;
    [SerializeField] private Tilemap m_BackgroundMap;

    #endregion

    #region Private variables

    /// <summary>
    /// Dictionary containing the HP value of each damaged tile.
    /// If a Tile position isn't in the Dictionary, the Tile either isn't damaged or doesn't exist.
    /// </summary>
    private Dictionary<Vector3Int, int> m_DamagedTiles = new Dictionary<Vector3Int, int>();

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        SharedInstance = this;

        TransformComponent = transform;
    }

    // TEST METHOD
    private void Update()
    {
        
        if (Input.GetMouseButtonUp(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int gridPosition = m_ForegroundMap.WorldToCell(mousePos);

                DamageTile(gridPosition);
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Converts a Cell position to the World position of its bottom-left corner.
    /// </summary>
    /// <param name="inPosition"></param>
    /// <returns></returns>
    public Vector3 CellToWorld(Vector3Int inPosition)
    {
        return m_ForegroundMap.CellToWorld(inPosition);
    }

    /// <summary>
    /// Converts a Cell position to the World position of its center.
    /// </summary>
    /// <param name="inPosition"></param>
    /// <returns></returns>
    public Vector3 CellToWorldCenter(Vector3Int inPosition)
    {
        return m_ForegroundMap.GetCellCenterWorld(inPosition);
    }

    /// <summary>
    /// Returns the Tile at the specified cell on the target Tilemap.
    /// </summary>
    /// <param name="inCell">Cell position.</param>
    /// <param name="inMap">Target Tilemap.</param>
    /// <returns></returns>
    public TDestructibleTile GetTile(Vector3Int inCell, TMap inMap = TMap.Foreground)
    {
        return GetTilemap(inMap)?.GetTile<TDestructibleTile>(inCell);
    }

    /// <summary>
    /// Sets the specified Tile at a cell position on the target Tilemap.
    /// </summary>
    /// <param name="inPosition">Cell position.</param>
    /// <param name="inTile">Tile to set.</param>
    /// <param name="inMap">Target Tilemap.</param>
    public void SetTile(Vector3Int inPosition, TDestructibleTile inTile, TMap inMap)
    {
        if (inMap == TMap.Foreground)
            m_ForegroundMap.SetTile(inPosition, inTile);
        else if (inMap == TMap.Background)
            m_BackgroundMap.SetTile(inPosition, inTile);
    }

    /// <summary>
    /// Damages the Tile at the specified cell on target Tilemap.
    /// </summary>
    /// <param name="inCell">Cell position.</param>
    /// <param name="inMap">Target Tilemap.</param>
    /// <param name="inDamage">Damage dealt.</param>
    public void DamageTile(Vector3Int inCell, TMap inMap = TMap.Foreground, int inDamage = 1)
    {

        // Get affected Tilemap
        Tilemap affectedMap;

        if (inMap == TMap.Foreground)
            affectedMap = m_ForegroundMap;
        else if (inMap == TMap.Background)
            affectedMap = m_BackgroundMap;
        else
            return;


        // Get affected Tile
        TDestructibleTile affectedTile = affectedMap.GetTile<TDestructibleTile>(inCell);

        // If the position is empty, return
        if (!affectedTile)
            return;
        
        // If the Tile has been damaged already, update current HP value
        if (m_DamagedTiles.ContainsKey(inCell))
        {
            int hitPoints = m_DamagedTiles[inCell];
            hitPoints -= inDamage;

            if (hitPoints <= 0)
            {
                // Call OnDestruction method
                affectedTile.DropContainedItems(m_ForegroundMap.GetCellCenterWorld(inCell));

                DestroyObjectsAt(inCell + Vector3Int.up);

                // Remove Tile from Tilemap and Damage dictionary
                m_ForegroundMap.SetTile(inCell, null);
                m_DamagedTiles.Remove(inCell);
            }
            else
            {
                // Update HP
                m_DamagedTiles[inCell] = hitPoints;
                Debug.Log("Damaged tile at pos: " + inCell);
            }
        }
        else
        {
            // Get the Tile
            TDestructibleTile tile = m_ForegroundMap.GetTile<TDestructibleTile>(inCell);

            // Init its HP to be stored by taking the damage into account
            int hitPoints = tile.HitPoints - inDamage;

            if (hitPoints <= 0)
            {
                // Call OnDestruction method
                affectedTile.DropContainedItems(m_ForegroundMap.CellToWorld(inCell));

                // Remove Tile from Tilemap
                m_ForegroundMap.SetTile(inCell, null);
            }
            else
            {
                // Add the Tile to the Damage dictionary
                m_DamagedTiles.Add(inCell, hitPoints);
                //Debug.Log("Damaged tile at pos: " + inCell);
            }
        }
    }

    /// <summary>
    /// Converts a World position to the corresponding position on the Tilemap's grid.
    /// </summary>
    /// <param name="inWorldPosition"></param>
    /// <returns></returns>
    public Vector3Int WorldToGridPosition(Vector3 inWorldPosition)
    {
        return m_ForegroundMap.WorldToCell(inWorldPosition);
    }

    /// <summary>
    /// Checks if the cell has any neighboring Tile on the target Tilemap (four directions + same cell on other Tilemap). 
    /// </summary>
    /// <param name="inCell">Cell position.</param>
    /// <param name="inMap">Target Tilemap.</param>
    /// <returns>True if at least one neighbor has been found.</returns>
    public bool CheckForNeighbors(Vector3Int inCell, TMap inMap)
    {
        // Determine target and other Tilemap
        Tilemap targetMap;
        Tilemap otherMap;

        if (inMap == TMap.Background)
        {
            targetMap = m_BackgroundMap;
            otherMap = m_ForegroundMap;
        }
        else if (inMap == TMap.Foreground)
        {
            targetMap = m_ForegroundMap;
            otherMap = m_BackgroundMap;
        }
        else
            return false;

        // Check all four direction on target Tilemap
        for (int i = 0; i < GridUtility.Directions.Length; i++)
        {
            if (targetMap.HasTile(inCell + GridUtility.Directions[i]))
                return true;
        }

        // If a neighbor wasn't found on target Tilemap, check on other Tilemap.
        return otherMap.HasTile(inCell);
    }

    /// <summary>
    /// Check if the specified cell may be considered grounded (meaning the cell below is occupied by a Tile).
    /// </summary>
    /// <param name="inCell">Cell position.</param>
    /// <returns></returns>
    public bool IsGrounded(Vector3Int inCell)
    {
        return m_ForegroundMap.HasTile(inCell + Vector3Int.down);
    }

    public bool IsOccupied(Vector3Int inCell, TMap inMap)
    {
        return GetTilemap(inMap).HasTile(inCell);
    }


    #endregion

    private Tilemap GetTilemap(TMap inMap)
    {
        if (inMap == TMap.Foreground)
            return m_ForegroundMap;
        else if (inMap == TMap.Background)
            return m_BackgroundMap;

        else return null;
    }

    private void DestroyObjectsAt(Vector3Int inCell)
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(m_ForegroundMap.GetCellCenterWorld(inCell), CellSize / 2, 0);

        for(int i = 0; i < hitColliders.Length; i++)
        {
            hitColliders[i].GetComponentInParent<TWorldItem>()?.Destroy();
        }
    }
}



public enum Direction
{
    Right = 0,
    Up = 1,
    Left = 2,
    Down = 3
}

public class GridUtility
{
    public static readonly Vector3Int[] Directions =
    {
        new Vector3Int(1,0,0),
        new Vector3Int(0,1,0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, -1, 0)
    };
}
