using UnityEngine;
using System.Collections;


/// <summary>
/// Player controller => It works as connection between model and view.
/// </summary>
[RequireComponent(typeof(TPlayerDefenseComponent))]
public class TPlayerController : BaseEntity, IKnockBackable, IJump, IMovable
{
    public static TPlayerController SharedInstance { get; private set; }


    /// <summary>
    /// Contain reference to item in player's hand.
    /// </summary>

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

    /// <summary>
    /// Inventory component
    /// </summary>

    private TInventory m_PlayerInventoryComponent;
    public TInventory PlayerInventoryComponent
    {
        get
        {
            if (m_PlayerInventoryComponent == null)
            {
                m_PlayerInventoryComponent = GetComponent<TInventory>();
            }
            return m_PlayerInventoryComponent;
        }
    }

    private TCraftingItemsComponent m_PlayerCraftComponent;
    public TCraftingItemsComponent PlayerCraftComponent
    {
        get
        {
            if(m_PlayerCraftComponent == null)
            {
                m_PlayerCraftComponent = GetComponent<TCraftingItemsComponent>();
            }
            return m_PlayerCraftComponent;
        }
    }

    /// <summary>
    /// health component
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

    #region Data

    /// <summary>
    /// Return a copy of the data.
    /// </summary>

    [SerializeField] private TPlayerData m_DataToAssign;

    private TPlayerData m_DataAssigned;
    public TPlayerData DataAssigned
    {
        get
        {
            if (m_DataAssigned == null)
                m_DataAssigned = Instantiate(m_DataToAssign);
            return m_DataAssigned;
        }
    }

    #endregion

    #region SerializeField

    /// Jump value / components

    [Header("Jump variables")]
    [SerializeField] private LayerMask m_JumpableLayers;
    [SerializeField] private Transform m_Legs;

    /// Weapon value / components

    [Header("Weapon variables")]
    [SerializeField] private PlayerAttack m_HandToAttack;

    // Animator

    [Header("Animator")]
    [SerializeField] private RuntimeAnimatorController m_DefaultAnim;
    [SerializeField] private Animator m_Anim;

    // Property, "get" only

    public Transform Legs => m_Legs;
    public Animator Anim => m_Anim;
    public float KbResist => DataAssigned.KbResist;


    #endregion

    // @TEMP
    private bool m_ImFreeze;
    private float m_FreezeTime;
    private Collider2D m_NcpColliderHit;

    #region MonoBehaviour cycle

    private void OnEnable()
    {
        // subscribe to equip event
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemSelected, OnItemSelected);
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemDeselected, OnItemDeselect);

        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemEquipped, OnItemEquipped);
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemUnequipped, OnItemUnequipped);
    }

    private void OnDisable()
    {
        // unsubscibe to equip event.
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemSelected, OnItemSelected);
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemDeselected, OnItemDeselect);

        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemEquipped, OnItemEquipped);
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemUnequipped, OnItemUnequipped);
    }

    private void Awake()
    {
        SharedInstance = this;
    }

    protected override void Start()
    {
        // Init components
        base.Start();
        m_HandToAttack.gameObject.SetActive(false);
        DefenseComponent.Init(DataAssigned.MaxHealth, DataAssigned.Defense);

        // Update visual
        TEventManager.TriggerEvent<IDefend>(TEventID.OnHealthUpdate, DefenseComponent);

        PlayerInventoryComponent.InventorySlotsReference();
        PlayerCraftComponent.CraftingButtonsReference();
    }

    private void Update()
    {
        Jump();

        if (m_ImFreeze)
        {
            m_FreezeTime -= Time.deltaTime;

            if (m_FreezeTime <= 0)
            {
                m_ImFreeze = false;
                Physics2D.IgnoreCollision(m_Collider, m_NcpColliderHit, false);
            }
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
            KnockBack(knockEffect);

            // Start freeze
            Freeze(0.6f);
        }
    }

    #endregion

    #region Movement

    /// <summary>
    /// Freeze player movement.
    /// Turn off collision with last enemy hit for an amount of time.
    /// </summary>
    public void Freeze(float inTime)
    {
        m_ImFreeze = true;
        m_FreezeTime = inTime;
        Physics2D.IgnoreCollision(m_Collider, m_NcpColliderHit, true);
    }
    
    /// <summary>
    /// Knock back effect, it's applied with gravity.
    /// </summary>
    /// <param name="direction"></param>
    public void KnockBack(Vector2 direction)
    {
        m_Rb.AddForce(direction);
    }

    /// <summary>
    /// Move the player if no walls are detected.
    /// It's called by INputManager.
    /// </summary>
    public void Move(float inDirection)
    {
        if (!m_ImFreeze && inDirection != 0)
        {
            if (!Physics2D.Raycast(m_Legs.position, Vector2.right * inDirection, 0.5f, m_JumpableLayers))
            {
                Vector2 scale = new Vector2(m_LocalScale.x * inDirection, m_LocalScale.y);

                m_Transform.localScale = scale; 
                transform.position += (Vector3.right * inDirection) * DataAssigned.Speed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Player jump, apply more gravity when falling.
    /// </summary>
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
        return Mathf.FloorToInt(Vector3Int.Distance(inCell, TTilemapManager.SharedInstance.WorldToGridPosition(m_Transform.position))) <= DataAssigned.MaxActionDistance;
    }

    #region Actions

    public void UseEquippedItem(TPointerData data)
    {
        if (PlayerItem.ItemInHand)
        {
            bool successful = false;

            if (PlayerItem.ItemInHand.ItemInSlot.Item is IUsable usable)
               successful = usable.Use(this, data);

            if (successful && PlayerItem.ItemInHand.ItemInSlot.Item.DepleteOnUse)
                PlayerItem.ItemInHand.DepleteAmount(1);
        }
    }

    public void Act(TPointerData data)
    {
        if (IsInActionRange(data.GridPosition))
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(CameraFollow.MainCamera.ScreenPointToRay(data.ScreenPosition));

            if (hit.collider)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null) interactable.Interact();
            }
        }
    }

    #endregion

    #region Weapon

    /// <summary>
    /// Turn on weapon in hand,
    /// It start to rotate or plunge.
    /// </summary>
    /// <returns></returns>
    public bool CanAttack()
    {
        if (!m_HandToAttack.gameObject.activeSelf)
        {
            m_HandToAttack.gameObject.SetActive(true);
            Anim.SetFloat("AttackMultiplier", DataAssigned.AttackSpeed);
            Anim.SetTrigger("Attack");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Turn off item in hand. It's called in animation event.
    /// </summary>
    public void TurnOffItemRoot()
    {
        m_HandToAttack.gameObject.SetActive(false);
    }

    /// <summary>
    /// Able to choose player attack type in animation event.
    /// </summary>
    public void SetAttackType(PlayerAttackType type)
    {
        m_HandToAttack.AttackType = type;
    }

    #endregion

    #region Select / Equip

    /// <summary>
    /// Set stats when an item is selected.
    /// </summary>
    public void OnItemSelected(TInventorySlot item)
    {
        // Is it a weapon?
        // Update: stats, layerMask, sprite, animator controller.s

        if (item.ItemInSlot.Item is TItemWeapon)
        {
            TItemWeapon weapon = item.ItemInSlot.Item as TItemWeapon;

            DataAssigned.Damage = m_DataToAssign.Damage + weapon.Attack;
            DataAssigned.AttackSpeed = m_DataToAssign.AttackSpeed + weapon.AttackSpeed;

            m_HandToAttack.Damage = m_DataToAssign.Damage + weapon.Attack;
            m_HandToAttack.AttackType = weapon.VisualAndInteraction.AttackType;
            m_HandToAttack.InteractableLayer = weapon.VisualAndInteraction.InteractableLayer;
            m_HandToAttack.WeaponIcon.sprite = weapon.ItemSprite;
            m_HandToAttack.gameObject.SetActive(false);

            m_Anim.runtimeAnimatorController = weapon.VisualAndInteraction.PlayerOverrideController;

        }
    }

    /// <summary>
    /// Set stats when an item is deselected.
    /// </summary>
    public void OnItemDeselect(TInventorySlot item)
    {
        if (!item) return;

        // Is it a weapon?
        // Update: stats, layerMask, sprite, animator controller.s

        if (item.ItemInSlot.Item is TItemWeapon)
        {
            TItemWeapon weapon = item.ItemInSlot.Item as TItemWeapon;

            DataAssigned.Damage = m_DataToAssign.Damage - weapon.Attack;
            DataAssigned.AttackSpeed = m_DataToAssign.AttackSpeed - weapon.AttackSpeed;

            m_HandToAttack.Damage = m_DataToAssign.Damage + weapon.Attack;
            m_HandToAttack.AttackType = PlayerAttackType.Melee;
            m_HandToAttack.InteractableLayer = LayerMask.NameToLayer("Default");
            m_HandToAttack.WeaponIcon.sprite = null;

            m_Anim.runtimeAnimatorController = m_DefaultAnim;
        }
    }

    /// <summary>
    /// Set stats when an item is equipped
    /// </summary>
    public void OnItemEquipped(TInventorySlot item)
    {
        // Update stats
        // Update sprites.

        if (item.ItemInSlot.Item is TItemArmor)
        {
            TItemArmor armor = item.ItemInSlot.Item as TItemArmor;

            // Init health and defense.

            DataAssigned.Defense = m_DataToAssign.Defense + armor.Defence;
            DefenseComponent.Init(DataAssigned.MaxHealth, DataAssigned.Defense);
        }
    }

    /// <summary>
    /// Set stats when an item is unequipped
    /// </summary>
    public void OnItemUnequipped(TInventorySlot item)
    {
        if (!item) return;

        // Update stats
        // Update sprites.

        if (item.ItemInSlot.Item is TItemArmor)
        {
            TItemArmor armor = item.ItemInSlot.Item as TItemArmor;

            // Init health and defense.

            DataAssigned.Defense = m_DataToAssign.Defense;
            DefenseComponent.Init(DataAssigned.MaxHealth, DataAssigned.Defense);
        }
    }

    #endregion
}
