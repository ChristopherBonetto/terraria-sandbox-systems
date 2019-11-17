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
        [SerializeField] private TItem[] m_ContainedItems;

        #endregion

        #region Public methods
        /// <summary>
        /// Places a Pickup for each contained Item at the center of the Tile.
        /// </summary>
        /// <param name="inWorldPosition"></param>
        public void DropContainedItems(Vector3 inWorldPosition)
        {
            Debug.Log("Destroyed tile at world pos: " + inWorldPosition);

            foreach(TItem containedItem in m_ContainedItems)
            {
                // Get Pickup object from the pool
                GameObject pickupObj = ObjectPooler.SharedInstance.GetPooledObject("Pickup");

                if (pickupObj)
                {
                    // Load Item
                    TItemPickup pickup = pickupObj.GetComponent<TItemPickup>();
                    pickup.LoadItem(containedItem);

                    // Set Pickup position
                    pickup.TransformComponent.position = inWorldPosition;

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
