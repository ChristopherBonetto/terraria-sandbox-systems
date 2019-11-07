using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TMap
{
    Background,
    Foreground,
    Placement
}

public class TTilemapManager : MonoBehaviour
{
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
    
    // TEST METHOD

    //private void Update()
    //{
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector3Int gridPosition = m_ForegroundMap.WorldToCell(mousePos);

    //        DamageTile(gridPosition);
    //    }
    //}

    #endregion

    #region Public methods

    /// <summary>
    /// Returns the Tile at the specified position on the requested Tilemap.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public TDestructibleTile GetTile(Vector3Int position, TMap map = TMap.Foreground)
    {
        if (map == TMap.Foreground)
            return m_ForegroundMap.GetTile<TDestructibleTile>(position);

        return null;
    }

    /// <summary>
    /// Damages the Tile at the specified position on the requested Tilemap.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="map"></param>
    /// <param name="damage"></param>
    public void DamageTile(Vector3Int position, TMap map = TMap.Foreground, int damage = 1)
    {

        // Get affected Tilemap
        Tilemap affectedMap;

        if (map == TMap.Foreground)
            affectedMap = m_ForegroundMap;
        else
            return;


        // Get affected Tile
        TDestructibleTile affectedTile = affectedMap.GetTile<TDestructibleTile>(position);

        // If the position is empty, return
        if (!affectedTile)
            return;
        
        // If the Tile has been damaged already, update current HP value
        if (m_DamagedTiles.ContainsKey(position))
        {
            int hitPoints = m_DamagedTiles[position];
            hitPoints -= damage;

            if (hitPoints <= 0)
            {
                // Call OnDestruction method
                affectedTile.OnDestruction(m_ForegroundMap.CellToWorld(position));

                // Remove Tile from Tilemap and Damage dictionary
                m_ForegroundMap.SetTile(position, null);
                m_DamagedTiles.Remove(position);
            }
            else
            {
                // Update HP
                m_DamagedTiles[position] = hitPoints;
                Debug.Log("Damaged tile at pos: " + position);
            }
        }
        else
        {
            // Get the Tile
            TDestructibleTile tile = m_ForegroundMap.GetTile<TDestructibleTile>(position);

            // Init its HP to be stored by taking the damage into account
            int hitPoints = tile.HitPoints - damage;

            if (hitPoints <= 0)
            {
                // Call OnDestruction method
                affectedTile.OnDestruction(m_ForegroundMap.CellToWorld(position));

                // Remove Tile from Tilemap
                m_ForegroundMap.SetTile(position, null);
            }
            else
            {
                // Add the Tile to the Damage dictionary
                m_DamagedTiles.Add(position, hitPoints);
                Debug.Log("Damaged tile at pos: " + position);
            }
        }
    }

    #endregion
}
