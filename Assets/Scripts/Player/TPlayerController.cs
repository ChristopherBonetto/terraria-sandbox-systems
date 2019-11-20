using UnityEngine;
using System.Collections;


/// <summary>
/// Player controller => It works as connection between model and view.
/// </summary>
[RequireComponent(typeof(TJumpComponent), 
                  typeof(TDefenseComponent), 
                  typeof(TLinearMovement))]

public class TPlayerController : MonoBehaviour
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

    private IMovable m_MovementComponent;
    /// <summary>
    /// Player's movement component
    /// </summary>
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
    /// <summary>
    /// Player's defense component
    /// </summary>
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
    /// <summary>
    /// Player's jump component
    /// </summary>
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

    private Rigidbody2D m_Rb;
    /// <summary>
    /// Player's rigid body.
    /// </summary>
    public Rigidbody2D Rb
    {
        get
        {
            if (m_Rb == null)
                m_Rb = GetComponent<Rigidbody2D>();
            return m_Rb;
        }
    }

    private Collider2D m_Collider;
    /// <summary>
    /// Player's rigid body.
    /// </summary>
    public Collider2D Collider
    {
        get
        {
            if (m_Collider == null)
                m_Collider = GetComponent<Collider2D>();
            return m_Collider;
        }
    }

    #endregion

    // @TEMP
    private bool m_ImFreeze;
    private float m_FreezeTime = 0.5f;

    private Collider2D m_NcpColliderHit;

    private void OnEnable()
    {
        // subscribe

        // Input
        TEventManager.SubscribeTo<float>(TEventID.OnMovementAxis, OnPlayerMovement);
        TEventManager.SubscribeTo(TEventID.OnJumpDOWN, OnPlayerJumpDown);
        TEventManager.SubscribeTo(TEventID.OnJumpUP, OnPlayerJumpUp);

        // view
        MovementComponent.OnMoveEvent += View.Flip;
    }

    private void OnDisable()
    {
        // unsubscripted

        // Input
        TEventManager.SubscribeTo<float>(TEventID.OnMovementAxis, OnPlayerMovement);
        TEventManager.SubscribeTo(TEventID.OnJumpDOWN, OnPlayerJumpDown);
        TEventManager.SubscribeTo(TEventID.OnJumpUP, OnPlayerJumpUp);

        // view
        MovementComponent.OnMoveEvent -= View.Flip;
    }

    private void Start()
    {
        DefenseComponent.Init(DataAssigned.MaxHealth, DataAssigned.Defense);
        MovementComponent.Init(DataAssigned.Speed);
        JumpComponent.Init(Rb, DataAssigned.JumpForce);

        // Update visual
        TEventManager.TriggerEvent<IDefend>(TEventID.OnHealthUpdate, DefenseComponent);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            Vector2 collidingPos = collision.transform.position;
            Vector2 myPos = transform.position;

            float sign = Mathf.Sign(myPos.x - collidingPos.x);

            Vector2 knockEffect = (Vector2.right * sign * GeneralEffects.KbEffect(DataAssigned.KbResist)) + Vector2.up * GeneralEffects.KbGlobalEffect * 0.5f;

            Rb.AddForce(knockEffect);

            if (m_NcpColliderHit != collision.collider || m_NcpColliderHit == null)
                m_NcpColliderHit = collision.collider;

            Physics2D.IgnoreCollision(Collider, m_NcpColliderHit, false);
            StartCoroutine("Stun", m_FreezeTime);
        }
    }

    public IEnumerator Stun(float stunTime)
    {
        m_ImFreeze = true;
        yield return new WaitForSeconds(stunTime);

        m_ImFreeze = false;
        Physics2D.IgnoreCollision(Collider, m_NcpColliderHit, false);
    }


    #region Input manager event methods

    private void OnPlayerMovement(float inDirection)
    {
        if (!m_ImFreeze)
        {

            // Movement
            inDirection = Input.GetAxisRaw("Horizontal");

            if (inDirection != 0)
            {
                MovementComponent.OnMovement(Vector2.right * inDirection);
            }
        }
    }

    private void OnPlayerJumpDown()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpComponent.OnJumpDecision(Vector2.up);
        }
    }

    private void OnPlayerJumpUp()
    {
        StartCoroutine("JumpUp");
    }

    IEnumerator JumpUp()
    {
        while (Rb.velocity.y > 0)
        {
            Rb.velocity += Vector2.up * (Physics2D.gravity.y + 9.0f);
            yield return null;
        }
    }

    #endregion
}
