using UnityEngine;
using System.Collections;

namespace Terrria.AI
{
    /// <summary>
    /// Hops in one direction, slides on slopes, floats in water, follows player if damaged or it's nighttime.
    /// </summary>
    [RequireComponent(typeof(JumpComponent))]
    public class SlimeAI : BaseAI
    {
        [SerializeField]
        private float m_JumpDelay;      // @TEMP
        private float m_LastJumpTime;   // @TEMP

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

            // if someday the model must contains other value like "jump force"
            // Init the "jump force here"
        }

        private void Update()
        {
            /*
             
             Jump system
             main notes: follow the player if it's night or it's damaged.

             Randomize a bunch of vectors with positive sign.

             Randomize the signs only for X axis,
             if it's chasing the player, get the sign of player direction.

            Recap:
            2 states [ Idle, Chasing ].
            Idle ==> randomize sign.
            Chasing ==> get sign.

            Bunch of Vectors (i'm not sure  about this but it's intresting)
             */

            if (Time.time > m_LastJumpTime + m_JumpDelay)
            {
                // @TEMP
                // Create possible slime's jump vector
                Vector2[] directions = {
                    new Vector2(1, 1),
                    new Vector2(-1, 1),
                    new Vector2(-0.6f, 1),
                    new Vector2(0.8f, 1),
                    new Vector2(-0.2f, 1),
                    new Vector2(0.5f, 1)
                };

                // randomize one (in line) and execute jump
                JumpComponent.Jump(directions[Random.Range(0, directions.Length)]);

                m_LastJumpTime = Time.time;
            }
        }
    }
}
