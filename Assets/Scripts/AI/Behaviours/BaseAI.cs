using UnityEngine;

namespace Terrria.AI
{
    /// <summary>
    /// Base AI,
    /// doesn't move or react.
    /// </summary>
    [RequireComponent(typeof(TDefenseComponent))]
    public class BaseAI : MonoBehaviour
    {
        [SerializeField] private AIBaseData m_Data;
        public AIBaseData Data => m_Data;

        // Component
        private IDefend m_DefenseComponent;
        [SerializeField] private Rigidbody2D m_Rb;
        [SerializeField] private Collider2D m_Collider;

        public IDefend DefenseComponent
        {
            get
            {
                if (m_DefenseComponent == null)
                    m_DefenseComponent = GetComponent<IDefend>();
                return m_DefenseComponent;
            }
        }
        public Rigidbody2D Rb => m_Rb;
        public Collider2D Collider => m_Collider;

        //Player reference 
        protected TPlayerController m_Player;


        protected virtual void Start()
        {
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
