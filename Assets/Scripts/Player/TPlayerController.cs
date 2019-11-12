using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TJumpComponent), typeof(TDefenseComponent), typeof(TLinearMovement))]
public class TPlayerController : MonoBehaviour
{
    #region Private
    [SerializeField]
    private float m_DistanceToDetection; //@TEMP
    #endregion

    #region Properties
    private IMovable m_MovementComponent;
    public IMovable MovementComponent
    {
        get
        {
            if (m_MovementComponent == null)
                m_MovementComponent = GetComponent<IMovable>();
            return m_MovementComponent;
        }
    }

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

    private TPlayerView m_View;
    public TPlayerView View
    {
        get
        {
            if (m_View == null)
                m_View = GetComponent<TPlayerView>();
            return m_View;
        }
    }


    [SerializeField]
    private float m_MaxHealth;
    public float MaxHealth => m_MaxHealth;
    #endregion


    private void OnEnable()
    {
        // subscribe
        MovementComponent.OnMove += View.Flip;
    }

    private void OnDisable()
    {
        // unsubscripted
        MovementComponent.OnMove -= View.Flip;
    }

    private void Update()
    {
        // Movement
        float direction = Input.GetAxisRaw("Horizontal");

        if (direction != 0)
        {
            MovementComponent.OnMovement(Vector2.right * direction);
        }   

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpComponent.OnJumpDecision(Vector2.up);
        }
        else if (!Input.GetKey(KeyCode.Space) && JumpComponent.Rb.velocity.y > 0)
        {
            JumpComponent.Rb.velocity += Vector2.up * (Physics2D.gravity.y + 9.5f);
        }
    }
}
