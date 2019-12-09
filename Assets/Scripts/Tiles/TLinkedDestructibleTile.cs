using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Tilemaps
{
    [CreateAssetMenu(menuName = "Tiles/Linked Destructible Tile")]
    public class TLinkedDestructibleTile : TDestructibleTile
    {
        #region Serialized variables

        [Header("Linking")]

        /// <summary>
        /// Directions in which to check for similar tiles to destroy.
        /// </summary> 
        [SerializeField] GridUtility.Direction[] m_DestructionDirections;

        /// <summary>
        /// Max number of units per pickup dropped.
        /// </summary> 
        [Tooltip("Max number of units per pickup dropped.")]
        [SerializeField] private int m_QuantityUnitsPerPickup;

        #endregion

        /// <summary>
        /// Executes the Tile's destruction, destroying linked tiles in the process.
        /// </summary>
        /// <param name="inTilemap">Tilemap in which this type of Tile is placed.</param>
        /// <param name="inTileCell">Tile coordinates.</param>
        public override void DestroySelf(Tilemap inTilemap, Vector3Int inTileCell)
        {
            // Get the list of linked cells
            List<Vector3Int> linkedCells = GetLinkedCellsAndRemoveTiles(inTilemap, inTileCell);

            int cellsCount = linkedCells.Count;

            // Calculate the pickups count based on settings, keeping the remainder of the division apart
            int pickupsCount = cellsCount / m_QuantityUnitsPerPickup;
            int remainder = cellsCount % m_QuantityUnitsPerPickup;

            int itemsCount = m_ContainedItems.Length;

            TItemQuantity currentItem;

            // Drop a pickup every m_QuantityUnitsPerPickup, containing m_QuantityUnitsPerPickup * preset-units units
            // Remainder is dropped on the first destroyed tile
            for (int i = 0; i < itemsCount; i++)
            {
                currentItem = m_ContainedItems[i];

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

        /// <summary>
        /// Gets cells linked to the provided starting cell and removes all linked Tiles from the Tilemap (Breadth First Search).
        /// </summary>
        /// <param name="inTilemap"></param>
        /// <param name="inStartCell"></param>
        /// <returns></returns>
        private List<Vector3Int> GetLinkedCellsAndRemoveTiles(Tilemap inTilemap, Vector3Int inStartCell)
        {
            // Cache directions in order to avoid continuous casting
            int dirCount = m_DestructionDirections.Length;

            Vector3Int[] dirs = new Vector3Int[dirCount];

            for (int i = 0; i < dirCount; i++)
                dirs[i] = GridUtility.Directions[(int)m_DestructionDirections[i]];

            // Init frontier
            Queue<Vector3Int> frontier = new Queue<Vector3Int>();
            frontier.Enqueue(inStartCell);

            // Init visited set
            Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>
            {
                { inStartCell, true }
            };

            Vector3Int current;
            Vector3Int next;

            // Keep searching till the frontier is emptied.
            while (frontier.Count > 0)
            {
                // Dequeue to get current
                current = frontier.Dequeue();

                // Remove it from Tilemap
                inTilemap.SetTile(current, null);
                TEventManager.TriggerEvent(TEventID.OnTileDestroyed, current);

                // Check in required directions to find valid neighbours
                for (int i = 0; i < dirCount; i++)
                {
                    next = current + dirs[i];

                    // If it's the same Tile type and it hasn't been visited yet, add it to the visited set and enqueue it in frontier
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
