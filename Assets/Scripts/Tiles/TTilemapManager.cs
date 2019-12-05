using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum TMap
{
    Foreground = 0,
    Midground = 1,
    Background = 2
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
    public Vector3 CellSize { get; private set; }

    /// <summary>
    /// Cached reference to the attached Transform component.
    /// </summary>
    public Transform TransformComponent { get; private set; }

    #endregion

    #region Serialized variables

    [SerializeField] private Tilemap[] m_Tilemaps;

    #endregion

    #region Private variables

    /// <summary>
    /// Dictionaries tracking the damage taken from each Tile. Each Dictionary keeps track of one Tilemap.
    /// If a Tile's coordinates aren't in the Dictionary, the Tile either isn't damaged or doesn't exist at that coordinates.
    /// </summary>
    private Dictionary<Vector3Int, int>[] m_TileDamage;

    private int m_TilemapsCount;

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        SharedInstance = this;

        TransformComponent = transform;

        m_TilemapsCount = m_Tilemaps.Length;

        CellSize = m_Tilemaps[0].cellSize;

        m_TileDamage = new Dictionary<Vector3Int, int>[m_TilemapsCount];

        for (int i = 0; i < m_TilemapsCount; i++) m_TileDamage[i] = new Dictionary<Vector3Int, int>();
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
        return m_Tilemaps[0].CellToWorld(inPosition);
    }

    /// <summary>
    /// Converts a Cell position to the World position of its center.
    /// </summary>
    /// <param name="inPosition"></param>
    /// <returns></returns>
    public Vector3 CellToWorldCenter(Vector3Int inPosition)
    {
        return m_Tilemaps[0].GetCellCenterWorld(inPosition);
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
        GetTilemap(inMap)?.SetTile(inPosition, inTile);
    }


    /// <summary>
    /// Damages the Tile at the specified position if belongs to one of the provided groups, prioritizing Foreground.
    /// </summary>
    /// <param name="inCell"></param>
    /// <param name="inGroupIDs"></param>
    /// <param name="inDamage"></param>
    public void TryDamageTile(Vector3Int inCell, List<TWorldGroupID> inGroupIDs, int inDamage = 1)
    {
        TDestructibleTile affectedTile;

        // Cycle over Tilemaps
        for (int i = 0; i < m_TilemapsCount; i++)
        {
            // Get affected Tile
            affectedTile = m_Tilemaps[i].GetTile<TDestructibleTile>(inCell);

            if (affectedTile)
            {
                // If the Tile's group is in the list of damageable groups, damage it
                if (inGroupIDs.Contains(affectedTile.GroupID))
                    DamageTile(i, inCell, inDamage);

                // If any Tile was found, regardless of it being damaged (= can't damage background through foreground)
                break;
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
        return m_Tilemaps[0].WorldToCell(inWorldPosition);
    }

    /// <summary>
    /// Checks if the cell has any neighboring Tile on the target Tilemap (four directions + same cell on other Tilemap). 
    /// </summary>
    /// <param name="inCell">Cell position.</param>
    /// <param name="inMap">Target Tilemap.</param>
    /// <returns>True if at least one neighbor has been found.</returns>
    public bool CheckForNeighbors(Vector3Int inCell, TMap inMap)
    {
        int mapIndex = (int)inMap;

        // Check all four direction on target Tilemap
        for (int i = Mathf.Max(0, mapIndex-1); i < Mathf.Min(m_TilemapsCount, mapIndex+1); i++)
        {
            if (i != mapIndex && m_Tilemaps[i].HasTile(inCell)) return true;

            for (int j = 0; j < GridUtility.Directions.Length; j++)
            {
                if (m_Tilemaps[i].HasTile(inCell + GridUtility.Directions[j]))
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the cell on the Tilemap is already occupied.
    /// </summary>
    /// <param name="inCell"></param>
    /// <param name="inMap"></param>
    /// <returns>True if it's occupied.</returns>
    public bool IsOccupied(Vector3Int inCell, TMap inMap)
    {
        Tilemap targetMap = GetTilemap(inMap);

        if (targetMap)
            return GetTilemap(inMap).HasTile(inCell);

        else
        {
            Debug.LogError("Required Tilemap " + inMap + " doesn't exist.");
            return false;
        }
    }

    public bool IsGrounded(Vector3Int inCell, TMap inMap)
    {
        int index = (int)inMap;

        for (int i = index; i > -1; i--)
        {
            if (m_Tilemaps[i].HasTile(inCell + Vector3Int.down))
                return true;
        }

        return false;
    }


    #endregion

    /// <summary>
    /// Damages the the Tile at the specified Tilemap cell.
    /// </summary>
    /// <param name="mapIndex">Index corresponding to the interested Tilemap</param>
    /// <param name="inCell">Cell coordinates.</param>
    /// <param name="inDamageDealt">Damage dealt.</param>
    private void DamageTile(int mapIndex, Vector3Int inCell, int inDamageDealt = 1)
    {
        TDestructibleTile tile = m_Tilemaps[mapIndex].GetTile<TDestructibleTile>(inCell);
        if (!tile) return;
        
        // CASE 1: The Tile was damaged before
        if (m_TileDamage[mapIndex].ContainsKey(inCell))
        {
            // Add new damage
            m_TileDamage[mapIndex][inCell] += inDamageDealt;

            // Check if the Tile should be destroyed (damage taken >= Tile HP)
            if (m_TileDamage[mapIndex][inCell] >= tile.HitPoints)
            {
                // Destroy Tile and remove it from damage tracking dictionary
                tile.DestroySelf(m_Tilemaps[mapIndex], inCell);
                m_TileDamage[mapIndex].Remove(inCell);

                TEventManager.TriggerEvent(TEventID.OnTileDestroyed, inCell);
            }
            else
            {
                TEventManager.TriggerEvent(TEventID.OnTileDamaged, inCell, m_TileDamage[mapIndex][inCell] / (float)tile.HitPoints);
            }
        }

        // CASE 2: the Tile hasn't been damaged yet, but the damage is enough to destroy it directly
        else if (inDamageDealt >= tile.HitPoints)
        {
            tile.DestroySelf(m_Tilemaps[mapIndex], inCell);
            TEventManager.TriggerEvent(TEventID.OnTileDestroyed, inCell);
        }

        // CASE 3: the Tile hasn't been damaged yet, but the damage is NOT enough to destroy it
        else
        {
            // Add the Tile position to damage tracking dictionary
            m_TileDamage[mapIndex].Add(inCell, inDamageDealt);
            TEventManager.TriggerEvent(TEventID.OnTileDamaged, inCell, inDamageDealt / (float)tile.HitPoints);
        }
    }

    /// <summary>
    /// Gets the required Tilemap instance.
    /// </summary>
    /// <param name="inMap"></param>
    /// <returns></returns>
    private Tilemap GetTilemap(TMap inMap)
    {
        int index = (int)inMap;

        if (index < 0 || index >= m_Tilemaps.Length)
        {
            Debug.LogError("The required Tilemap " + inMap + " does not exist.");
            return null;
        }

        return m_Tilemaps[index];
    }
}

/// <summary>
/// Utility class for Grid navigation.
/// </summary>
public class GridUtility
{
    public enum Direction
    {
        Right = 0,
        Up = 1,
        Left = 2,
        Down = 3
    }

    public static readonly Vector3Int[] Directions =
    {
        Vector3Int.right,
        Vector3Int.up,
        Vector3Int.left,
        Vector3Int.down
    };
}
