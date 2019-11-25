using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Tilemaps
{
    [CreateAssetMenu(menuName = "Tiles/Linked Destructible Tile")]
    public class TLinkedDestructibleTile : TDestructibleTile
    {
        [SerializeField] GridUtility.Direction[] m_DestructionDirections;

        /// <summary>
        /// Max number of units per pickup dropped.
        /// </summary> 
        [Tooltip("Max number of units per pickup dropped.")]
        [SerializeField] private int m_QuantityUnitsPerPickup;

        public override void DestroySelf(Tilemap inTilemap, Vector3Int inTileCell)
        {
            List<Vector3Int> linkedCells = GetLinkedCellsAndRemoveTiles(inTilemap, inTileCell);

            int tilesCount = linkedCells.Count;

            int pickupsCount = tilesCount / m_QuantityUnitsPerPickup;
            int remainder = tilesCount % m_QuantityUnitsPerPickup;

            int itemsCount = m_ContainedItems.Length;

            for (int i = 0; i < itemsCount; i++)
            {
                TItemQuantity currentItem = m_ContainedItems[i];

                currentItem.Amount *= m_QuantityUnitsPerPickup;

                for (int j = 0; j < pickupsCount; j++)
                    SpawnPickup(currentItem, linkedCells[j * m_QuantityUnitsPerPickup]);

                if (remainder != 0)
                {
                    currentItem.Amount = m_ContainedItems[i].Amount * remainder;
                    SpawnPickup(currentItem, inTileCell + Vector3Int.up);
                }
            }
        }

        private List<Vector3Int> GetLinkedCellsAndRemoveTiles(Tilemap inTilemap, Vector3Int inStartCell)
        {
            int dirCount = m_DestructionDirections.Length;

            Vector3Int[] dirs = new Vector3Int[dirCount];

            for (int i = 0; i < dirCount; i++)
                dirs[i] = GridUtility.Directions[(int)m_DestructionDirections[i]];

            Queue<Vector3Int> frontier = new Queue<Vector3Int>();
            frontier.Enqueue(inStartCell);

            Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>
            {
                { inStartCell, true }
            };

            Vector3Int current;
            Vector3Int next;

            while (frontier.Count > 0)
            {
                current = frontier.Dequeue();

                inTilemap.SetTile(current, null);

                for (int i = 0; i < dirCount; i++)
                {
                    next = current + dirs[i];

                    if (inTilemap.GetTile<TLinkedDestructibleTile>(next) == this && !visited.ContainsKey(next))
                    {
                        visited.Add(next, true);
                        frontier.Enqueue(next);
                    }
                }
            }

            return new List<Vector3Int>(visited.Keys);
        }
    }
}
