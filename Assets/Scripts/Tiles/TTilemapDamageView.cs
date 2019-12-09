using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Tilemaps
{
    [RequireComponent(typeof(Tilemap))]
    public class TTilemapDamageView : MonoBehaviour
    {
        /// <summary>
        /// Tiles used to visualize Tile damage.
        /// </summary>
        [SerializeField] private Tile[] m_CrackTiles;

        #region Private variables

        /// <summary>
        /// Damage level step amplitude.
        /// </summary>
        private float m_DamageLevelStep;

        /// <summary>
        /// Amount of damage levels between in [0,1] (0% - 100%)
        /// </summary>
        private int m_DamageLevelsCount;

        /// <summary>
        /// Tilemap on which to visualize Tile damage.
        /// </summary>
        private Tilemap m_Tilemap;

        #endregion

        #region MonoBehaviour cycle

        private void Awake()
        {
            m_Tilemap = GetComponent<Tilemap>();
        }

        private void Start()
        {
            // Damage levels count = amount of possible different views
            m_DamageLevelsCount = m_CrackTiles.Length;

            // Equally divide the [0,1] interval between levels
            m_DamageLevelStep = 1.0f / m_DamageLevelsCount;
        }

        private void OnEnable()
        {
            // Subscribe to events
            TEventManager.SubscribeTo<Vector3Int, float>(TEventID.OnTileDamaged, SetDamaged);
            TEventManager.SubscribeTo<Vector3Int>(TEventID.OnTileDestroyed, ClearDamage);
        }

        private void OnDisable()
        {
            // Unsubcribe from events
            TEventManager.UnsubscribeFrom<Vector3Int, float>(TEventID.OnTileDamaged, SetDamaged);
            TEventManager.UnsubscribeFrom<Vector3Int>(TEventID.OnTileDestroyed, ClearDamage);
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Sets the damage view at the specified cell.
        /// </summary>
        /// <param name="inCell">Cell on which to the set the view.</param>
        /// <param name="inDamagePercentage">Percentage of the damage as ' total-damage / max-hit-points '</param>
        public void SetDamaged(Vector3Int inCell, float inDamagePercentage)
        {
            // Check each level and set the correct Tile view
            for (int i = 1; i < m_DamageLevelsCount; i++)
            {
                if (inDamagePercentage < i * m_DamageLevelStep)
                {
                    m_Tilemap.SetTile(inCell, m_CrackTiles[i - 1]);
                    return;
                }
            }

            m_Tilemap.SetTile(inCell, m_CrackTiles[m_DamageLevelsCount - 1]);
        }

        /// <summary>
        /// Removes the view at the specified cell.
        /// </summary>
        /// <param name="inCell"></param>
        public void ClearDamage(Vector3Int inCell)
        {
            m_Tilemap.SetTile(inCell, null);
        }

        #endregion
    }
}

