using UnityEngine;
using System.Collections;


/// <summary>
/// Player controller => It works as connection between model and view.
/// </summary>
[RequireComponent(typeof(TDefenseComponent))]
public class TPlayerController : MonoBehaviour, IKnockBackable, IJump, IMovable
{
    private TItemInHandComponent m_PlayerItem;
    public TItemInHandComponent PlayerItem
    {
        get
        {
            if (m_PlayerItem == null)
            {
                m_PlayerItem = GetComponent<TItemInHandComponent>();
            }                
            return m_PlayerItem;
        }            
    }

    private TInventory m_PlayerInventory;
    public TInventory PlayerInventory
    {
        get
        {
            if (m_PlayerInventory == null)
            {
                m_PlayerInventory = GetComponent<TInventory>();
            }
            return m_PlayerInventory;
        }
    }

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
    private IDefend m_DefenseComponent;

    [Header("Components")]

    [SerializeField] private Rigidbody2D m_Rb;
    [SerializeField] private Collider2D m_Collider;
    [SerializeField] private Transform m_Transform;
    private Vector3 m_LocalScale;

    [Header("Jump variables")]

    [SerializeField] private LayerMask m_JumpableLayers;
    [SerializeField] private Transform m_Legs;

    // Player's component (get only)
    public IDefend DefenseComponent
    {
        get
        {
            if (m_DefenseComponent == null)
                m_DefenseComponent = GetComponent<IDefend>();
            return m_DefenseComponent;
        }
    }
    public Rigidbody2D Rb => m_Rb;
    public Collider2D Collider => m_Collider;
    public Transform Transform => m_Transform;

    public LayerMask JumpableLayers => m_JumpableLayers;
    public Transform Legs => m_Legs;

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
        m_LocalScale = Transform.localScale;

        // Update visual
        TEventManager.TriggerEvent<IDefend>(TEventID.OnHealthUpdate, DefenseComponent);
    }

    private void Update()
    {
        Jump();

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

            // Execute knock back
            Rb.AddForce(knockEffect);
            // Start freeze
            Freeze(0.5f);
            // ignore collision
            Physics2D.IgnoreCollision(Collider, m_NcpColliderHit, false);
        }
    }

    public void Freeze(float inTime)
    {
        m_ImFreeze = true;
        m_FreezeTime = inTime;
        Physics2D.IgnoreCollision(Collider, m_NcpColliderHit, false);
    }

    public void Move(float inDirection)
    {
        if (!m_ImFreeze && inDirection != 0)
        {
                transform.position += (Vector3.right * inDirection) * DataAssigned.Speed * Time.deltaTime;

                // View
                Vector2 scale = new Vector2(m_LocalScale.x * inDirection, m_LocalScale.y);
                Transform.localScale = scale; 
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics2D.Raycast(Legs.position, Vector2.down + Vector2.right * Collider.bounds.extents.x, 0.3f, JumpableLayers) ||
                Physics2D.Raycast(Legs.position, Vector2.down + Vector2.right * -Collider.bounds.extents.x, 0.3f, JumpableLayers))
                    Rb.AddForce(Vector2.up * DataAssigned.JumpForce);
        }

        else if (!Input.GetKey(KeyCode.Space))
        {
            if ((Rb.velocity.y > 0))
                Rb.velocity += Vector2.up * (Physics2D.gravity.y + 9.0f);
        }

    }

    public bool IsInActionRange(Vector3Int inCell)
    {
        return Mathf.CeilToInt(Vector3Int.Distance(inCell, TTilemapManager.SharedInstance.WorldToGridPosition(Transform.position))) <= DataAssigned.MaxActionDistance;
    }

    public void UseEquippedItem(TPointerData data)
    {
        if (PlayerItem.ItemInHand)
        {
            bool successful = PlayerItem.ItemInHand.ItemInSlot.Item.Use(this, data);

            if (successful && TItemHandler.SharedInstance.SelectedItem.ItemInSlot.Item.DepleteOnUse)
                PlayerItem.ItemInHand.DepleteAmount(1);
        }
    }
}
