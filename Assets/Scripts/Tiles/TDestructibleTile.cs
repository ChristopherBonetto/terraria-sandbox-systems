using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Tilemaps
{
    [CreateAssetMenu(menuName = "Tiles/Destructible Tile")]
    public class TDestructibleTile : RuleTile
    {
        public int HitPoints { get { return m_HitPoints; } }

        [SerializeField] private int m_HitPoints;

        // [SerializeFIeld] private TItem m_ContainedItems;

        public void OnDestruction(Vector3 tilePosition)
        {
            Debug.Log("Destroyed tile at world pos: " + tilePosition);
        }
    }
}
