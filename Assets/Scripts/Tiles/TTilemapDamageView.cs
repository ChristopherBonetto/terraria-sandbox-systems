using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Tilemaps
{
    [RequireComponent(typeof(Tilemap))]
    public class TTilemapDamageView : MonoBehaviour
    {
        [SerializeField] private Tile[] m_CrackTiles;

        private float m_DamageLevelStep;
        private int m_DamageLevelsCount;
        private Tilemap m_Tilemap;

        private void Awake()
        {
            m_Tilemap = GetComponent<Tilemap>();
        }

        private void Start()
        {
            m_DamageLevelsCount = m_CrackTiles.Length;

            m_DamageLevelStep = 1.0f / m_DamageLevelsCount;
        }

        private void OnEnable()
        {
            TEventManager.SubscribeTo<Vector3Int, float>(TEventID.OnTileDamaged, SetDamaged);
            TEventManager.SubscribeTo<Vector3Int>(TEventID.OnTileDestroyed, ClearDamage);
        }

        private void OnDisable()
        {
            TEventManager.UnsubscribeFrom<Vector3Int, float>(TEventID.OnTileDamaged, SetDamaged);
            TEventManager.UnsubscribeFrom<Vector3Int>(TEventID.OnTileDestroyed, ClearDamage);
        }

        public void SetDamaged(Vector3Int inCell, float damagePercentage)
        {
            for (int i = 1; i < m_DamageLevelsCount; i++)
            {
                if (damagePercentage < i * m_DamageLevelStep)
                {
                    m_Tilemap.SetTile(inCell, m_CrackTiles[i - 1]);
                    return;
                }
            }

            m_Tilemap.SetTile(inCell, m_CrackTiles[m_DamageLevelsCount - 1]);
        }

        public void ClearDamage(Vector3Int inCell)
        {
            m_Tilemap.SetTile(inCell, null);
        }
    }
}

