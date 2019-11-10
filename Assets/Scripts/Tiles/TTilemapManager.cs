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

    #region Serialized variables

    [SerializeField] private Tilemap m_ForegroundMap;

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
    /// Returns the Tile at the specified position on the requested Tilemap.
    /// </summary>
    /// <param name="inPosition"></param>
    /// <param name="inMap"></param>
    /// <returns></returns>
    public TDestructibleTile GetTile(Vector3Int inPosition, TMap inMap = TMap.Foreground)
    {
        if (inMap == TMap.Foreground)
            return m_ForegroundMap.GetTile<TDestructibleTile>(inPosition);

        return null;
    }

    /// <summary>
    /// Damages the Tile at the specified position on the requested Tilemap.
    /// </summary>
    /// <param name="inPosition"></param>
    /// <param name="inMap"></param>
    /// <param name="damage"></param>
    public void DamageTile(Vector3Int inPosition, TMap inMap = TMap.Foreground, int damage = 1)
    {

        // Get affected Tilemap
        Tilemap affectedMap;

        if (inMap == TMap.Foreground)
            affectedMap = m_ForegroundMap;
        else
            return;


        // Get affected Tile
        TDestructibleTile affectedTile = affectedMap.GetTile<TDestructibleTile>(inPosition);

        // If the position is empty, return
        if (!affectedTile)
            return;
        
        // If the Tile has been damaged already, update current HP value
        if (m_DamagedTiles.ContainsKey(inPosition))
        {
            int hitPoints = m_DamagedTiles[inPosition];
            hitPoints -= damage;

            if (hitPoints <= 0)
            {
                // Call OnDestruction method
                affectedTile.DropContainedItems(m_ForegroundMap.GetCellCenterWorld(inPosition));

                // Remove Tile from Tilemap and Damage dictionary
                m_ForegroundMap.SetTile(inPosition, null);
                m_DamagedTiles.Remove(inPosition);
            }
            else
            {
                // Update HP
                m_DamagedTiles[inPosition] = hitPoints;
                Debug.Log("Damaged tile at pos: " + inPosition);
            }
        }
        else
        {
            // Get the Tile
            TDestructibleTile tile = m_ForegroundMap.GetTile<TDestructibleTile>(inPosition);

            // Init its HP to be stored by taking the damage into account
            int hitPoints = tile.HitPoints - damage;

            if (hitPoints <= 0)
            {
                // Call OnDestruction method
                affectedTile.DropContainedItems(m_ForegroundMap.CellToWorld(inPosition));

                // Remove Tile from Tilemap
                m_ForegroundMap.SetTile(inPosition, null);
            }
            else
            {
                // Add the Tile to the Damage dictionary
                m_DamagedTiles.Add(inPosition, hitPoints);
                Debug.Log("Damaged tile at pos: " + inPosition);
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

    #endregion
}
