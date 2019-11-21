using UnityEngine;
using System.Collections;


/// <summary>
/// Player controller => It works as connection between model and view.
/// </summary>
[RequireComponent(typeof(TJumpComponent), 
                  typeof(TDefenseComponent), 
                  typeof(TLinearMovement))]

public class TPlayerController : MonoBehaviour, IKnockBackable
{
    #region Data

    [SerializeField]
    private TPlayerData m_DataToAssign;
    /// <summary>
    /// Default player's stats
    /// </summary>
    public TPlayerData DataToAssign { get { return m_DataToAssign; } }

    private TPlayerData m_DataAssigned;
    /// <summary>
    /// Return a copy of the data.
    /// </summary>
    public TPlayerData DataAssigned
    {
        get
        {
            if (m_DataAssigned == null)
                m_DataAssigned = Instantiate(DataToAssign);
            return m_DataAssigned;
        }
    }

    #endregion

    #region Components

    // Player's component (must be assigned)
    private IMovable m_MovementComponent;
    private IDefend m_DefenseComponent;
    private IJump m_JumpComponent;
    [SerializeField] private Rigidbody2D m_Rb;
    [SerializeField] private Collider2D m_Collider;

    // Player's component (get only)
    public IMovable MovementComponent
    {
        get
        {
            if (m_MovementComponent == null)
                m_MovementComponent = GetComponent<IMovable>();
            return m_MovementComponent;
        }
    }
    public IDefend DefenseComponent
    {
        get
        {
            if (m_DefenseComponent == null)
                m_DefenseComponent = GetComponent<IDefend>();
            return m_DefenseComponent;
        }
    }
    public IJump JumpComponent
    {
        get
        {
            if (m_JumpComponent == null)
                m_JumpComponent = GetComponent<IJump>();
            return m_JumpComponent;
        }
    }
    public Rigidbody2D Rb => m_Rb;
    public Collider2D Collider => m_Collider;

    private TPlayerView m_View;
    /// <summary>
    /// Player view.
    /// It's used to show armor sprites and other visual.
    /// </summary>
    public TPlayerView View
    {
        get
        {
            if (m_View == null)
                m_View = GetComponent<TPlayerView>();
            return m_View;
        }
    }

    #endregion

    // @TEMP
    private bool m_ImFreeze;
    private float m_FreezeTime;

    private Collider2D m_NcpColliderHit;

    private void Start()
    {
        DefenseComponent.Init(DataAssigned.MaxHealth, DataAssigned.Defense);
        MovementComponent.Init(DataAssigned.Speed);
        JumpComponent.Init(Rb, DataAssigned.JumpForce);

        // Update visual
        TEventManager.TriggerEvent<IDefend>(TEventID.OnHealthUpdate, DefenseComponent);
    }

    private void Update()
    {
        PlayerMovement();
        PlayerJump();

        if (m_ImFreeze)
        {
            m_FreezeTime -= Time.deltaTime;

            if (m_FreezeTime <= 0)
                m_ImFreeze = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            m_NcpColliderHit = collision.collider;

            Vector2 collidingPos = collision.transform.position;
            Vector2 myPos = transform.position;

            float sign = Mathf.Sign(myPos.x - collidingPos.x);

            Vector2 knockEffect = (Vector2.right * sign * GeneralEffects.KbEffect(DataAssigned.KbResist)) +
                                    Vector2.up * GeneralEffects.KbGlobalEffect * 0.5f;

            // Execute knockback
            Rb.AddForce(knockEffect);
            // Start freeze
            Freeze(0.5f);
            // ignore collision
            Physics2D.IgnoreCollision(Collider, m_NcpColliderHit, false);
        }
    }

    private void PlayerMovement()
    {
        if (!m_ImFreeze)
        {
            var inDirection = Input.GetAxisRaw("Horizontal");

            if (inDirection != 0)
            {
                MovementComponent.Move(Vector2.right * inDirection);
            }
        }
    }

    private void PlayerJump()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpComponent.Jump(Vector2.up);
        }

        else if (!Input.GetKey(KeyCode.Space))
        {
            if ((Rb.velocity.y > 0))
                Rb.velocity += Vector2.up * (Physics2D.gravity.y + 9.0f);
        }
    }

    public void Freeze(float inTime)
    {
        m_ImFreeze = true;
        m_FreezeTime = inTime;
        Physics2D.IgnoreCollision(Collider, m_NcpColliderHit, false);
    }
}
