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

        #region Private
        private IJump m_JumpComponent;
        private Rigidbody2D m_Rb;

        [SerializeField] private float m_JumpDelay;      // @TEMP

        private float m_LastJumpTime;   // @TEMP
        private Transform m_Target;
        private Vector2[] m_PossibleDirections;
        #endregion

        #region Properties
        public IJump JumpComponent
        {
            get
            {
                if (m_JumpComponent == null)
                    m_JumpComponent = GetComponent<IJump>();
                return m_JumpComponent;
            }
        }
        public Rigidbody2D Rb
        {
            get
            {
                if (m_Rb == null)
                    m_Rb = GetComponent<Rigidbody2D>();
                return m_Rb;
            }
        }
        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();

            // subscrive method on change day/night event.
        }

        protected override void Start()
        {
            base.Start();

            // if someday the model must contains other value like "jump force"
            // Init the "jump force here"

            m_PossibleDirections = new Vector2[CreateJumpVectors().Length];
            m_PossibleDirections = CreateJumpVectors();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // unsubscrive method on change day/night event.
        }

        private void Update()
        {
            if (Time.time > m_LastJumpTime + m_JumpDelay)
            {
                // Pick a vector
                Vector2 direction = m_PossibleDirections[Random.Range(0, m_PossibleDirections.Length)];


                switch (CurrentState)
                {
                    case SlimeState.Idle:
                        bool isPositive = Random.Range(0, 2) == 1;

                        if (!isPositive)
                            direction.x = -direction.x;

                        // Execute jump
                        JumpComponent.OnJumpDecision(direction);
                        break;

                    case SlimeState.Chasing:
                        float sign = Mathf.Sign(-10);

                        // Execute jump
                        JumpComponent.OnJumpDecision(direction * sign);
                        break;
                }

                m_LastJumpTime = Time.time;
            }
        }

        public void SetState(SlimeState state)
        {
            CurrentState = state;

            //if (state == SlimeState.Chasing)
                // m_Target = GameManager.Insatce.Player;
            // else
                // m_Target = null
        }

        private Vector2[] CreateJumpVectors()
        {
            Vector2[] directions = {
                    new Vector2(1, 1),
                    new Vector2(0.6f, 1),
                    new Vector2(0.8f, 1),
                    new Vector2(0.5f, 1)
                };

            return directions;
        }
    }
}
