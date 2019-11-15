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

    #region Components

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

    #endregion

    #region Weapon handler

    [SerializeField]
    private TItemScriptable m_ItemInHand;
    public TItemScriptable ItemInHand
    {
        get { return m_ItemInHand; }
        set
        {
            m_ItemInHand = value;

            // reset stats
            if (m_ItemInHand == null)
            {
                DefenseComponent.Init(Data.MaxHealth, Data.Defense);
                MovementComponent.Init(Data.Speed);
                JumpComponent.Init(Rb, Data.JumpForce);
            }
        }
    }

    #endregion


    private void OnEnable()
    {
        // subscribe

        // Input
        TInputManager.SharedInstance.OnMovementAxis += OnPlayerMovement;
        TInputManager.SharedInstance.OnJumpDown += OnPlayerJumpDown;
        TInputManager.SharedInstance.OnJumpUp += OnPlayerJumpUp;

        // model
        DefenseComponent.OnDamageEvent += DefenseComponent.TakeDamage;
        MovementComponent.OnMoveEvent += MovementComponent.Move;
        JumpComponent.OnJumpEvent += JumpComponent.Jump;

        // view
        MovementComponent.OnMoveEvent += View.Flip;
        DefenseComponent.OnDamageEvent += View.UpdateHealthBar;
    }

    private void OnDisable()
    {
        // unsubscripted

        // Input
        TInputManager.SharedInstance.OnMovementAxis -= OnPlayerMovement;
        TInputManager.SharedInstance.OnJumpDown -= OnPlayerJumpDown;
        TInputManager.SharedInstance.OnJumpUp -= OnPlayerJumpUp;

        // model
        DefenseComponent.OnDamageEvent -= DefenseComponent.TakeDamage;
        MovementComponent.OnMoveEvent -= MovementComponent.Move;
        JumpComponent.OnJumpEvent -= JumpComponent.Jump;

        // view
        MovementComponent.OnMoveEvent -= View.Flip;
        DefenseComponent.OnDamageEvent -= View.UpdateHealthBar;
    }

    private void Start()
    {
        DefenseComponent.Init(Data.MaxHealth, Data.Defense);
        MovementComponent.Init(Data.Speed);
        JumpComponent.Init(Rb, Data.JumpForce);

        m_View.HealthBar.minValue = 0;
        m_View.HealthBar.maxValue = Data.MaxHealth;
        m_View.HealthBar.value = Data.MaxHealth;
    }

    #region Input manager event methods

    private void OnPlayerMovement(float inDirection)
    {
        // Movement
        inDirection = Input.GetAxisRaw("Horizontal");

        if (inDirection != 0)
        {
            MovementComponent.OnMovement(Vector2.right * inDirection);
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

    #region Generic event manager methods

    private void OnItemEquipped(TItemScriptable item)
    {
        ItemInHand = item;

        // set stats as default + item
        DefenseComponent.Init(Data.MaxHealth, Data.Defense + item.Defence);
        MovementComponent.Init(Data.Speed);
        JumpComponent.Init(Rb, Data.JumpForce);
    }

    #endregion
}
