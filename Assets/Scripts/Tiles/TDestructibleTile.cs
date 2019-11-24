using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Tilemaps
{
    [CreateAssetMenu(menuName = "Tiles/Destructible Tile")]
    public class TDestructibleTile : RuleTile
    {
        #region Public properties
        /// <summary>
        /// How many times the Tile should be hit to be destroyed.
        /// </summary>
        public int HitPoints { get { return m_HitPoints; } }

        /// <summary>
        /// Group of which this tile is part of.
        /// </summary>
        public TWorldGroupID GroupID { get { return m_GroupID; } }

        #endregion

        #region Serialized variables
        /// <summary>
        /// How many times the Tile should be hit to be destroyed.
        /// </summary>
        [Tooltip("How many times the Tile should be hit to be destroyed.")]
        [SerializeField] private int m_HitPoints;

        /// <summary>
        /// Items contained inside the Tile.
        /// </summary>
        [Tooltip("Items contained inside the Tile.")]
        [SerializeField] private TItemQuantity[] m_ContainedItems;

        /// <summary>
        /// Group of which this tile is part of.
        /// </summary>
        [Tooltip("Group of which this tile is part of.")]
        [SerializeField] private TWorldGroupID m_GroupID;

        #endregion

        #region Public methods
        /// <summary>
        /// Executes the Tile's destruction.
        /// </summary>
        /// <param name="inTilemap">Tilemap in which this type of Tile is placed.</param>
        /// <param name="inTileCell">Tile coordinates.</param>
        public void DestroySelf(Tilemap inTilemap, Vector3Int inTileCell)
        {
            // Get cell center's World coordinates
            Vector3 cellCenter = inTilemap.GetCellCenterWorld(inTileCell);

            // Destroy any WorldItem on top of the Tile.
            DestroyItemsOnTop(cellCenter, inTilemap.cellSize);

            // Remove Tile from Tilemap
            inTilemap.SetTile(inTileCell, null);

            // Drop Pickups containing the Items contained inside the Tile
            DropPickups(cellCenter);
        }

        /// <summary>
        /// Checks for World Items ontop of the Tile and destroys them.
        /// </summary>
        /// <param name="inCellCenter">Center of the Tile's cell.</param>
        /// <param name="inCellSize">Size of the cell.</param>
        protected void DestroyItemsOnTop(Vector3 inCellCenter, Vector3 inCellSize)
        {
            // Search for World Items
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(inCellCenter + Vector3.up * inCellSize.y, inCellSize / 2, 0);

            // If found, destroy them
            for (int i = 0; i < hitColliders.Length; i++)
                hitColliders[i].GetComponentInParent<TWorldItem>()?.Destroy();
        }

        /// <summary>
        /// Spawns a Pickup for each contained Item at the specified World position.
        /// </summary>
        /// <param name="inDropPosition"></param>
        protected void DropPickups(Vector3 inDropPosition)
        {
            foreach (TItemQuantity containedItem in m_ContainedItems)
            {
                // Get Pickup object from the pool
                GameObject pickupObj = ObjectPooler.SharedInstance.GetPooledObject("Pickup");

                if (pickupObj)
                {
                    // Load Item
                    TItemPickup pickup = pickupObj.GetComponent<TItemPickup>();
                    pickup.LoadItem(containedItem);

                    // Set Pickup position
                    pickup.TransformComponent.position = inDropPosition;

                    // Show Pickup
                    pickupObj.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Pool doesn't contain any available Pickup object.");
                    break;
                }
            }
        }
    }

    #endregion
}
