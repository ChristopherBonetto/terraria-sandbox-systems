using UnityEngine;
using System.Collections;

/// <summary>
/// Player controller => It works as connection between model and view.
/// </summary>
[RequireComponent(typeof(TJumpComponent), 
                  typeof(TDefenseComponent), 
                  typeof(TLinearMovement))]
[RequireComponent(typeof(TAttackComponent))]

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

    private IAttack m_AttackComponent;
    /// <summary>
    /// Player's Attack Component.
    /// </summary>
    public IAttack AttackComponent
    {
        get
        {
            if (m_AttackComponent == null)
                m_AttackComponent = GetComponent<IAttack>();
            return m_AttackComponent;
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

    #endregion

    #region Weapon handler

    [SerializeField]
    private TItemWeapon m_WeaponInHand;
    public TItemWeapon WeaponInHand
    {
        get { return m_WeaponInHand; }
        set
        {
            m_WeaponInHand = value;
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

        // view
        MovementComponent.OnMoveEvent -= View.Flip;
        DefenseComponent.OnDamageEvent -= View.UpdateHealthBar;
    }

    private void Start()
    {
        DefenseComponent.Init(DataAssigned.MaxHealth, DataAssigned.Defense);
        MovementComponent.Init(DataAssigned.Speed);
        JumpComponent.Init(Rb, DataAssigned.JumpForce);
        AttackComponent.Init(DataAssigned.Damage);

        m_View.HealthBar.minValue = 0;
        m_View.HealthBar.maxValue = DataAssigned.MaxHealth;
        m_View.HealthBar.value = DataAssigned.MaxHealth;
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

    private void OnItemEquipped(TItemWeapon item)
    {
        //WeaponInHand = item;

        //// set stats as default + item
        //DefenseComponent.Init(DataAssigned.MaxHealth, DataAssigned.Defense + item.Defence);
        //AttackComponent.Init(DataAssigned.Damage + item.Attack);
    }

    #endregion
}
