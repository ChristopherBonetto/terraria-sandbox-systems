using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TPlayerDefenseComponent))]
public class TPlayerController : BaseEntity, IKnockBackable, IJump, IMovable
{
    #region SerializeField

    [SerializeField] private TPlayerData m_DataToAssign;


    [Header("Jump variables")]
    [SerializeField] private LayerMask m_JumpableLayers;
    [SerializeField] private Transform m_Legs;


    [Header("Weapon variables")]
    [SerializeField] private PlayerAttack m_HandToAttack;


    [Header("Animator")]
    [SerializeField] private RuntimeAnimatorController m_DefaultAnim;
    [SerializeField] private Animator m_Anim;


    [Header("Equipment")]
    [Tooltip("0 = head, 1 = arms, 2 = chest, 3 = legs")]
    [SerializeField] private TArmorView[] m_ArmorsView;

    #endregion

    #region Private

    private TPlayerData m_DataAssigned;

    private TItemInHandComponent m_PlayerItem;
    private TInventory m_PlayerInventoryComponent;
    private TCraftingItemsComponent m_PlayerCraftComponent;

    private IDefend m_DefenseComponent;

    private bool m_ImFreeze;
    private float m_FreezeTime;

    private Collider2D m_NpcColliderHit;

    #endregion

    #region Property

    /// <summary>
    /// Return a copy of the data.
    /// </summary>
    public TPlayerData DataAssigned
    {
        get
        {
            if (m_DataAssigned == null)
                m_DataAssigned = Instantiate(m_DataToAssign);
            return m_DataAssigned;
        }
    }
    public float KbResist => DataAssigned.Statistics.KbResist;

    /// <summary>
    /// health component
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

    /// <summary>
    /// Contain reference to item in player's hand.
    /// </summary>
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

    public Transform Legs => m_Legs;
    public Animator Anim => m_Anim;

    #endregion

    #region MonoBehaviour cycle

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemSelected, OnItemSelected);
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemDeselected, OnItemDeselect);

        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemEquipped, OnItemEquipped);
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemUnequipped, OnItemUnequipped);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemSelected, OnItemSelected);
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemDeselected, OnItemDeselect);

        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemEquipped, OnItemEquipped);
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemUnequipped, OnItemUnequipped);
    }

    protected override void Start()
    {
        Init();

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
                Physics2D.IgnoreCollision(m_Collider, m_NpcColliderHit, false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            m_NpcColliderHit = collision.collider;

            // Find collision position.
            Vector2 collidingPos = collision.transform.position;
            Vector2 myPos = transform.position;


            // calculate the sign of the knock back direction
            float sign = Mathf.Sign(myPos.x - collidingPos.x);

            Vector2 knockEffect = (
                Vector2.right * sign * GeneralEffects.KbEffect(KbResist) +  // Apply the resist to knock back effect.
                Vector2.up * GeneralEffects.KbEffect(KbResist)
            );

            // Execute knock back
            KnockBack(knockEffect);

            // Start freeze
            Freeze(0.6f);
        }
    }

    #endregion


    /// <summary>
    /// initialization of the player.
    /// </summary>
    public void Init()
    {
        base.Start();   // store local scale.

        m_HandToAttack.gameObject.SetActive(false);

        DefenseComponent.Init(DataAssigned.Statistics.MaxHealth, DataAssigned.Statistics.Defense);

        TEventManager.TriggerEvent<IDefend>(TEventID.OnHealthUpdate, DefenseComponent); // Update health
    }

    #region Movement

    /// <summary>
    /// Freeze player movement.
    /// Turn off collision with last enemy hit for an amount of time.
    /// </summary>
    public void Freeze(float inTime)
    {
        m_ImFreeze = true;
        m_FreezeTime = inTime;
        Physics2D.IgnoreCollision(m_Collider, m_NpcColliderHit, true);
    }
    
    /// <summary>
    /// Knock back effect, it's applied with RigidBody.
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
                // Update local scale
                Vector2 scale = new Vector2(m_LocalScale.x * inDirection, m_LocalScale.y);
                m_Transform.localScale = scale; 

                transform.position += (Vector3.right * inDirection) * DataAssigned.Statistics.Speed * Time.deltaTime;
            }
        }
        SetAnimToPlay("IsMoving", inDirection != 0);
    }

    /// <summary>
    /// Player jump, apply more gravity when falling.
    /// </summary>
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 leftLeg = (Vector2)Legs.position + (Vector2.right * m_Collider.bounds.extents.x);
            Vector2 righttLeg = (Vector2)Legs.position + (Vector2.right * -m_Collider.bounds.extents.x);

            if (Physics2D.Raycast(leftLeg, Vector2.down, 0.3f, m_JumpableLayers) ||
                Physics2D.Raycast(righttLeg, Vector2.down, 0.3f, m_JumpableLayers))
                    m_Rb.AddForce(Vector2.up * DataAssigned.Statistics.JumpForce);
        }

        else if (!Input.GetKey(KeyCode.Space) || m_Rb.velocity.y < 0)
        {
            // Reduce gravity effect applied.
            float gravity = (Physics2D.gravity.y + 9.5f);
            m_Rb.velocity += Vector2.up * gravity;
        }

        SetAnimToPlay("IsJumping", m_Rb.velocity.y > 0);
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
            Anim.SetFloat("AttackMultiplier", DataAssigned.Statistics.AttackSpeed);
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

    #endregion

    #region Select / Equip

    /// <summary>
    /// Set stats when an item is selected.
    /// </summary>
    public void OnItemSelected(TInventorySlot item)
    {
        // Is it a weapon?

        if (item.ItemInSlot.Item is TItemWeapon)
        {
            TItemWeapon weapon = item.ItemInSlot.Item as TItemWeapon;

            // Add statistics.
            DataAssigned.Statistics += weapon.Statistics;

            // Update attack animation values.
            m_HandToAttack.Damage = DataAssigned.Statistics.Damage;
            m_HandToAttack.AttackType = weapon.VisualAndInteraction.AttackType;
            m_HandToAttack.InteractableLayer = weapon.VisualAndInteraction.InteractableLayer;
            m_HandToAttack.WeaponIcon.sprite = weapon.ItemSprite;
            m_HandToAttack.gameObject.SetActive(false);

            // Override the animator controller
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

        if (item.ItemInSlot.Item is TItemWeapon)
        {
            TItemWeapon weapon = item.ItemInSlot.Item as TItemWeapon;

            // Add statistics
            DataAssigned.Statistics -= weapon.Statistics;

            // Update attack animations values
            m_HandToAttack.Damage = DataAssigned.Statistics.Damage;
            m_HandToAttack.AttackType = PlayerAttackType.Melee;
            m_HandToAttack.InteractableLayer = LayerMask.NameToLayer("Default");
            m_HandToAttack.WeaponIcon.sprite = null;

            // Override the animator controller
            m_Anim.runtimeAnimatorController = m_DefaultAnim;
        }
    }

    /// <summary>
    /// Set stats when an item is equipped
    /// </summary>
    public void OnItemEquipped(TInventorySlot item)
    {
        if (item.ItemInSlot.Item is TItemArmor)
        {
            TItemArmor armor = item.ItemInSlot.Item as TItemArmor;

            // Update statistics
            DataAssigned.Statistics += armor.Statistics;
            DefenseComponent.UpdateDefenseStats(DataAssigned.Statistics.MaxHealth, DataAssigned.Statistics.Defense);

            // View
            int typeToInt = (int)armor.ArmorType;
            m_ArmorsView[typeToInt].SetSprite(armor.ItemSprite);
            m_ArmorsView[typeToInt].SetAnimator(armor.ArmorAnim);
        }
    }

    /// <summary>
    /// Set stats when an item is unequipped
    /// </summary>
    public void OnItemUnequipped(TInventorySlot item)
    {
        if (!item) return;

        if (item.ItemInSlot.Item is TItemArmor)
        {
            TItemArmor armor = item.ItemInSlot.Item as TItemArmor;

            // Update statistics
            DataAssigned.Statistics -= armor.Statistics;
            DefenseComponent.UpdateDefenseStats(DataAssigned.Statistics.MaxHealth, DataAssigned.Statistics.Defense);

            // View
            int typeToInt = (int)armor.ArmorType;
            m_ArmorsView[typeToInt].ResteValues();
        }
    }

    #endregion

    #region Animation

    /// <summary>
    /// Play the animation in common between player's animators.
    /// ex : call jump animation => legs and chest play jump animation.
    /// </summary>
    /// <param name="inName"></param>
    /// <param name="inValue"></param>
    private void SetAnimToPlay(string inName, bool inValue)
    {
        foreach (var anim in m_ArmorsView)
        {
            anim.SetAnim(inName, inValue);
        }
    }

    #endregion
}
