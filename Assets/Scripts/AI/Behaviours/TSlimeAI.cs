using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Terraria.AI
{
    public enum SlimeState
    {
        Idle,
        Chasing
    }

    /// <summary>
    /// Hops in one direction, slides on slopes, floats in water, follows player if damaged.
    /// </summary>
    public class TSlimeAI : BaseAI, IJump, IKnockBackable
    {
        [SerializeField] private float m_DelayBetweenJump;

        #region Private

        // Jump variables
        private float m_ActualDelay;
        private float m_LastJumpTime; 
        private Vector2 m_JumpVector;

        // Player ref.
        private Transform m_TargetToChase;

        #endregion

        public float KbResist => Data.KbResist;
        public SlimeState CurrentState { get; private set; }


        protected override void Start()
        {
            base.Start();

            m_ActualDelay = m_DelayBetweenJump;
            m_JumpVector = new Vector2(1, 0.8f);
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
            // Jump right or left if is not angry.
            // Follow the player once collide with him.

            switch (CurrentState)
            {
                case SlimeState.Idle:
                    bool isPositive = Random.Range(0, 2) == 1;

                    if (!isPositive)
                        m_JumpVector.x = -m_JumpVector.x;

                    m_Rb.AddForce(m_JumpVector * Data.JumpForce);
                    break;

                case SlimeState.Chasing:
                    var direction = (m_Player.transform.position - transform.position).normalized;
                    var fixDir = new Vector2(Mathf.Abs(m_JumpVector.x) * Mathf.Sign(direction.x), m_JumpVector.y);

                    m_Rb.AddForce(fixDir * Data.JumpForce);
                    break;
            }
        }

        public void Freeze(float inTime)
        {
            // No freeze
        }

        public void KnockBack(Vector2 direction)
        {
            m_Rb.AddForce(direction);
        }
    }
}
