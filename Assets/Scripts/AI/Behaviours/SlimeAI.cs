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
    public class SlimeAI : BaseAI, IJump
    {
        public SlimeState CurrentState { get; private set; }


        [SerializeField] private float m_DelayBetweenJump;      // @TEMP

        private float m_ActualDelay;

        private float m_LastJumpTime;   // @TEMP
        private Transform m_TargetToChase;
        private Vector2 m_Direction;

        protected override void Start()
        {
            base.Start();

            m_ActualDelay = m_DelayBetweenJump;
            m_Direction = new Vector2(1, 0.8f);
        }

        private void Update()
        {
            if (Time.time > m_LastJumpTime + m_ActualDelay)
            {
                Jump();

                m_ActualDelay = Random.Range(
                    m_DelayBetweenJump - 1.5f,
                    m_DelayBetweenJump + 1.5f
                    );

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

        public void Jump()
        {
            switch (CurrentState)
            {
                case SlimeState.Idle:
                    bool isPositive = Random.Range(0, 2) == 1;

                    if (!isPositive)
                        m_Direction.x = -m_Direction.x;

                    Rb.AddForce(m_Direction * Data.JumpForce);
                    break;

                case SlimeState.Chasing:
                    float sign = Mathf.Sign(m_Player.transform.position.x - transform.position.x);
                    var fixDir = new Vector2(m_Direction.x * sign, m_Direction.y);

                    Rb.AddForce(fixDir * Data.JumpForce);
                    break;
            }
        }
    }
}
