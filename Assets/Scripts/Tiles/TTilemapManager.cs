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
    [SerializeField] private Tilemap m_ForegroundMap;

    private Dictionary<Vector3Int, int> m_DamagedTiles = new Dictionary<Vector3Int, int>();

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = m_ForegroundMap.WorldToCell(mousePos);

            DamageTile(gridPosition);
        }
    }

    public TDestructibleTile GetTile(Vector3Int position, TMap map = TMap.Foreground)
    {
        if (map == TMap.Foreground)
            return m_ForegroundMap.GetTile<TDestructibleTile>(position);

        return null;
    }

    public void DamageTile(Vector3Int position, TMap map = TMap.Foreground, int damage = 1)
    {
        Tilemap affectedMap;

        if (map == TMap.Foreground)
            affectedMap = m_ForegroundMap;
        else
            return;

        TDestructibleTile affectedTile = affectedMap.GetTile<TDestructibleTile>(position);

        if (!affectedTile)
            return;
        
        if (m_DamagedTiles.ContainsKey(position))
        {
            int hitPoints = m_DamagedTiles[position];
            hitPoints -= damage;

            if (hitPoints <= 0)
            {
                affectedTile.OnDestruction(m_ForegroundMap.CellToWorld(position));
                m_ForegroundMap.SetTile(position, null);
                m_DamagedTiles.Remove(position);
            }
            else
            {
                m_DamagedTiles[position] = hitPoints;
                Debug.Log("Damaged tile at pos: " + position);
            }
        }
        else
        {
            TDestructibleTile tile = m_ForegroundMap.GetTile<TDestructibleTile>(position);
            int hitPoints = tile.HitPoints - 1;

            if (hitPoints <= 0)
            {
                affectedTile.OnDestruction(m_ForegroundMap.CellToWorld(position));
                m_ForegroundMap.SetTile(position, null);
            }
            else
            {
                m_DamagedTiles.Add(position, hitPoints);
                Debug.Log("Damaged tile at pos: " + position);
            }

        }
    }
}
