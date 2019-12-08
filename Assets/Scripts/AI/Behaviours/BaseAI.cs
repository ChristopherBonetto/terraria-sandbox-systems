using UnityEngine;

namespace Terraria.AI
{
    /// <summary>
    /// Base AI,
    /// doesn't move or react.
    /// </summary>
    [RequireComponent(typeof(TEnemyDefenseComponent))]
    public class BaseAI : BaseEntity
    {
        [SerializeField] private AIBaseData m_Data;

        private IDefend m_DefenseComponent;
        protected TPlayerController m_Player;   //Player reference

        public AIBaseData Data => m_Data;
        public IDefend DefenseComponent
        {
            get
            {
                if (m_DefenseComponent == null)
                    m_DefenseComponent = GetComponent<IDefend>();
                return m_DefenseComponent;
            }
        }


        protected override void Start()
        {
            base.Start();

            DefenseComponent.Init( Data.Statistics.MaxHealth, Data.Statistics.Defense);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (m_Player == null)
                    m_Player = collision.gameObject.GetComponent<TPlayerController>();

                m_Player.DefenseComponent?.TakeDamage(Data.Statistics.Damage);
            }
        }
    }
}
