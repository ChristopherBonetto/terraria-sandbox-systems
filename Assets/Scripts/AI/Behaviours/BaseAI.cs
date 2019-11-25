using UnityEngine;

namespace Terrria.AI
{
    /// <summary>
    /// Base AI,
    /// doesn't move or react.
    /// </summary>
    [RequireComponent(typeof(TEnemyDefenseComponent))]
    public class BaseAI : BaseEntity
    {
        [SerializeField] private AIBaseData m_Data;
        public AIBaseData Data => m_Data;

        // Component
        private IDefend m_DefenseComponent;

        public IDefend DefenseComponent
        {
            get
            {
                if (m_DefenseComponent == null)
                    m_DefenseComponent = GetComponent<IDefend>();
                return m_DefenseComponent;
            }
        }

        //Player reference 
        protected TPlayerController m_Player;


        protected override void Start()
        {
            base.Start();

            DefenseComponent.Init(/*inMaxHealth:*/ Data.MaxHealth, /*inDefense:*/ Data.Defense);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (m_Player == null)
                    m_Player = collision.gameObject.GetComponent<TPlayerController>();

                m_Player.DefenseComponent.TakeDamage(1);
            }
        }
    }
}
