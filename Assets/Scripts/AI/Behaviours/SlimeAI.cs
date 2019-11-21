using UnityEngine;
using System.Collections;

namespace Terrria.AI
{
    public enum SlimeState
    {
        Idle,
        Chasing
    }

    /// <summary>
    /// Hops in one direction, slides on slopes, floats in water, follows player if damaged or it's nighttime.
    /// </summary>
    [RequireComponent(typeof(TJumpComponent))]
    public class SlimeAI : BaseAI
    {
        public SlimeState CurrentState { get; private set; }


        [SerializeField] private float m_JumpDelay;      // @TEMP

        private float m_LastJumpTime;   // @TEMP
        private Transform m_TargetToChase;
        private Vector2 m_Direction;

        // Components
        private IJump m_JumpComponent;
        public IJump JumpComponent
        {
            get
            {
                if (m_JumpComponent == null)
                    m_JumpComponent = GetComponent<IJump>();
                return m_JumpComponent;
            }
        }


        protected override void Start()
        {
            base.Start();

            JumpComponent.Init(Rb, 80f);

            m_Direction = new Vector2(1, 0.8f);
        }

        private void Update()
        {
            if (Time.time > m_LastJumpTime + m_JumpDelay)
            {
                switch (CurrentState)
                {
                    case SlimeState.Idle:
                        bool isPositive = Random.Range(0, 2) == 1;

                        if (!isPositive)
                            m_Direction.x = -m_Direction.x;

                        // Execute jump
                        JumpComponent.OnJumpDecision(m_Direction);
                        break;

                    case SlimeState.Chasing:
                        float sign = Mathf.Sign(m_Player.transform.position.x - transform.position.x);
                        Vector2 fixDirection = new Vector2(m_Direction.x * sign, m_Direction.y);

                        // Execute jump
                        JumpComponent.OnJumpDecision(fixDirection);
                        break;
                }

                m_LastJumpTime = Time.time;
            }
        }

        public void SetState(SlimeState state)
        {
            CurrentState = state;
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                base.OnCollisionEnter2D(collision);

                SetState(SlimeState.Chasing);
            }
        }
    }
}
