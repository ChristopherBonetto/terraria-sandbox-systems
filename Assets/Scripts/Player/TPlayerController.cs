using UnityEngine;
using System.Collections;


/// <summary>
/// Player controller => It works as connection between model and view.
/// </summary>
[RequireComponent(typeof(TDefenseComponent))]
public class TPlayerController : BaseEntity, IKnockBackable, IJump, IMovable
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

    private IDefend m_DefenseComponent;


    [Header("Jump variables")]

    [SerializeField] private LayerMask m_JumpableLayers;
    [SerializeField] private Transform m_Legs;

    [Header("Weapon variables")]

    [SerializeField] private GameObject m_ItemRootAnimation;
    [SerializeField] private SpriteRenderer m_ItemInHandIcon;

    [Header("Animator")]

    [SerializeField] private Animator m_Anim;


    public IDefend DefenseComponent
    {
        get
        {
            if (m_DefenseComponent == null)
                m_DefenseComponent = GetComponent<IDefend>();
            return m_DefenseComponent;
        }
    }
    public Transform Legs => m_Legs;
    public SpriteRenderer ItemInHandIcon => m_ItemInHandIcon;
    public Animator Anim => m_Anim;

    #endregion

    // @TEMP
    private bool m_ImFreeze;
    private float m_FreezeTime;
    private Collider2D m_NcpColliderHit;


    protected override void Start()
    {
        base.Start();
        DefenseComponent.Init(DataAssigned.MaxHealth, DataAssigned.Defense);

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
            m_Rb.AddForce(knockEffect);
            // Start freeze
            Freeze(0.5f);
            // ignore collision
            Physics2D.IgnoreCollision(m_Collider, m_NcpColliderHit, false);
        }
    }

    #region Movement

    public void Freeze(float inTime)
    {
        m_ImFreeze = true;
        m_FreezeTime = inTime;
        Physics2D.IgnoreCollision(m_Collider, m_NcpColliderHit, false);
    }

    public void Move(float inDirection)
    {
        if (!m_ImFreeze && inDirection != 0)
        {
            if (!Physics2D.Raycast(m_Legs.position, Vector2.right * inDirection, 0.5f, m_JumpableLayers))
            {
                transform.position += (Vector3.right * inDirection) * DataAssigned.Speed * Time.deltaTime;

                // View
                Vector2 scale = new Vector2(m_LocalScale.x * inDirection, m_LocalScale.y);
                m_Transform.localScale = scale; 
            }
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics2D.Raycast(Legs.position, Vector2.down + Vector2.right * m_Collider.bounds.extents.x, 0.3f, m_JumpableLayers) ||
                Physics2D.Raycast(Legs.position, Vector2.down + Vector2.right * -m_Collider.bounds.extents.x, 0.3f, m_JumpableLayers))
                    m_Rb.AddForce(Vector2.up * DataAssigned.JumpForce);
        }

        else if (!Input.GetKey(KeyCode.Space))
        {
            if ((m_Rb.velocity.y > 0))
                m_Rb.velocity += Vector2.up * (Physics2D.gravity.y + 9.0f);
        }

    }

    #endregion

    public bool IsInActionRange(Vector3Int inCell)
    {
        return Mathf.CeilToInt(Vector3Int.Distance(inCell, TTilemapManager.SharedInstance.WorldToGridPosition(m_Transform.position))) <= DataAssigned.MaxActionDistance;
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

    #region Weapon

    public bool CanAttack()
    {
        if (!m_ItemRootAnimation.activeSelf)
        {
            m_ItemRootAnimation.SetActive(true);
            Anim.SetTrigger("Attack");
            return true;
        }
        return false;
    }

    public void TurnOffItemRoot()
    {
        m_ItemRootAnimation.SetActive(false);
    }

    #endregion
}
