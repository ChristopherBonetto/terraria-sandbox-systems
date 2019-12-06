using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terraria.AI
{
    public class TFighterAI : BaseAI, IJump, IKnockBackable, IMovable
    {
        /// <summary>
        /// SerializeField
        /// </summary>

        [Header("Animator")]
        [SerializeField] private Animator m_Anim;

        [Header("Jump variables")]
        [SerializeField] private Transform m_Legs;
        [SerializeField] private LayerMask m_JumpableLayers;
        [SerializeField] private float m_JumpCoolDown;

        [Header("Detection")]
        [SerializeField] private LayerMask m_PlayerMask;
        [SerializeField] private float m_Radius;

        /// <summary>
        /// Private
        /// </summary>

        private bool m_ImFreeze;
        private float m_CurrentFreezeTime;
        private Collider2D m_PlayerCollider;

        // Jump Fix

        private float m_CurrentJumpCoolDown;
        private bool m_ImMoving;

        /// <summary>
        /// Properties
        /// </summary>

        public float KbResist => Data.KbResist;


        private void Update()
        {
            if (!m_ImFreeze)
            {
                m_PlayerCollider = Physics2D.OverlapCircle(m_Transform.position, m_Radius, m_PlayerMask);

                if (m_PlayerCollider)
                {
                    if (m_Player == null)
                        m_Player = m_PlayerCollider.GetComponent<TPlayerController>();

                    float direction = Mathf.Sign(m_PlayerCollider.transform.position.x - m_Transform.position.x);

                    Move(direction);
                    Jump();
                }
            }
        }

        public void Freeze(float inTime)
        {
            m_CurrentFreezeTime = inTime;
        }

        public void Jump()
        {
            if (!m_ImMoving)
            {
                if (CanJump())
                {
                    // if there are something between enemy and player
                    if (Physics2D.Raycast(m_Transform.position, m_Player.transform.position - m_Transform.position, m_Radius, m_JumpableLayers))
                    {
                        // if it's touching the ground
                        // Jumps
                        if (Physics2D.Raycast(m_Legs.position + (Vector3.right * m_Collider.bounds.extents.x), Vector2.down, 0.1f, m_JumpableLayers) ||
                            Physics2D.Raycast(m_Legs.position + (Vector3.right * -m_Collider.bounds.extents.x), Vector2.down, 0.1f, m_JumpableLayers))
                            m_Rb.AddForce(Vector2.up * Data.JumpForce);
                    }
                }
            }
        }

        public void KnockBack(Vector2 direction)
        {
            m_Rb.AddForce(direction);
        }

        public void Move(float inDirection)
        {
            if (!Physics2D.Raycast(m_Legs.position, Vector2.right * inDirection, m_Collider.bounds.extents.x + 0.1f, m_JumpableLayers))
            {
                Vector2 scale = new Vector2(m_LocalScale.x * -inDirection, m_LocalScale.y);

                m_Transform.localScale = scale;
                transform.position += (Vector3.right * inDirection) * Data.Speed * Time.deltaTime;

                m_ImMoving = true;
            }
            else
                m_ImMoving = false;

            m_Anim.SetBool("IsMoving", m_ImMoving);
        }

        /// <summary>
        /// Fix multiple addforce in short time.
        /// </summary>
        /// <returns></returns>
        private bool CanJump()
        {
            if (m_CurrentJumpCoolDown <= 0)
            {
                m_CurrentJumpCoolDown = m_JumpCoolDown;
                return true;
            }
            else
            {
                m_CurrentJumpCoolDown -= Time.deltaTime;
                return false;
            }
        }
    }
}
