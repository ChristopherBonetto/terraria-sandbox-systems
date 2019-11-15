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
    /// <summary>
    /// Default player's stats
    /// </summary>
    [SerializeField]
    private TPlayerData m_Data;
    public TPlayerData Data { get { return m_Data; } }

    /// <summary>
    /// Player's movement component
    /// </summary>
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

    /// <summary>
    /// Player's defense component
    /// </summary>
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

    /// <summary>
    /// Player's jump component
    /// </summary>
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

    /// <summary>
    /// Player view.
    /// It's used to show armor sprites and other visual.
    /// </summary>
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

    /// <summary>
    /// Player's rigid body.
    /// </summary>
    private Rigidbody2D m_Rb;
    public Rigidbody2D Rb
    {
        get
        {
            if (m_Rb == null)
                m_Rb = GetComponent<Rigidbody2D>();
            return m_Rb;
        }
    }


    private void OnEnable()
    {
        // subscribe
        MovementComponent.OnMoveEvent += View.Flip;
        DefenseComponent.OnDamageEvent += View.UpdateHealthBar;
    }

    private void OnDisable()
    {
        // unsubscripted
        MovementComponent.OnMoveEvent -= View.Flip;
        DefenseComponent.OnDamageEvent -= View.UpdateHealthBar;
    }

    private void Start()
    {
        DefenseComponent.Init(Data.MaxHealth);
        MovementComponent.Init(Data.Speed);
        JumpComponent.Init(Rb, Data.JumpForce);

        m_View.HealthBar.minValue = 0;
        m_View.HealthBar.maxValue = Data.MaxHealth;
        m_View.HealthBar.value = Data.MaxHealth;
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
