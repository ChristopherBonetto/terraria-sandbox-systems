using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class TWorldItem : MonoBehaviour 
{
    #region Public properties

    /// <summary>
    /// Sprite used for the WorldItem's rendering.
    /// </summary>
    public Sprite ItemSprite
    {
        get
        {
            if (!SpriteRendererComponent) SpriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
            return SpriteRendererComponent.sprite;
        }
    }

    /// <summary>
    /// Cached reference to the attached Transform component.
    /// </summary>
    public Transform TransformComponent { get; private set; }

    /// <summary>
    /// Cached reference to the Renderer component attached to the GameObject or one of its children.
    /// </summary>
    public SpriteRenderer SpriteRendererComponent { get; private set; }

    /// <summary>
    /// Item corresponding to this World Item.
    /// </summary>
    public TItemWorldObject ReferenceItem { get; set; }

    /// <summary>
    /// Layer on which the Item is placed.
    /// </summary>
    public TMap PlacingLayer { get; set; }

    /// <summary>
    /// World Groud to which the World Item belongs to.
    /// </summary>
    public TWorldGroupID GroupID { get { return m_GroupID; } }

    #endregion

    #region Serialize variables

    /// <summary>
    /// Max number of hit points the Item can have.
    /// </summary>
    [SerializeField] private int m_MaxHitPoints;

    /// <summary>
    /// World Groud to which the World Item belongs to.
    /// </summary>
    [SerializeField] private TWorldGroupID m_GroupID;

    [Space]

    /// <summary>
    /// Item corresponding to this World Item.
    /// </summary>
    [SerializeField] private TItemWorldObject m_DefaultReferenceItem;

    /// <summary>
    /// Layer on which the Item is placed.
    /// </summary>
    [SerializeField] private TMap m_DefaultPlacingLayer;


    #endregion

    #region Private variables

    private int m_HitPoints;

    #endregion

    #region MonoBehaviour cycle

    protected virtual void Awake()
    {
        // Cache components references
        TransformComponent = transform;

        if(!SpriteRendererComponent) SpriteRendererComponent = GetComponentInChildren<SpriteRenderer>();

        // Set default layer
        PlacingLayer = m_DefaultPlacingLayer;
    }

    protected virtual void Start()
    {
        // Set hit points
        m_HitPoints = m_MaxHitPoints;

        // Set default reference item
        if (!ReferenceItem) ReferenceItem = m_DefaultReferenceItem;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Deals damage to the Item.
    /// </summary>
    /// <param name="inDamageDealt"></param>
    public void TakeDamage(int inDamageDealt = 1)
    {
        m_HitPoints -= inDamageDealt;

        if (m_HitPoints <= 0)
            Destroy();
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Destroys the Item.
    /// </summary>
    public void Destroy()
    {
        // If there's a reference Item, drop a pickup containing it at the center of the World Item
        if (ReferenceItem)
        {
            TItemPickup pickup = ObjectPooler.SharedInstance.GetPooledObject("Pickup").GetComponent<TItemPickup>();

            pickup.LoadItem(new TItemQuantity(ReferenceItem));

            pickup.TransformComponent.position = TransformComponent.position + TTilemapManager.SharedInstance.CellSize.x * ReferenceItem.Size.x * Vector3.right / 2
                                                                             + TTilemapManager.SharedInstance.CellSize.y * ReferenceItem.Size.y * Vector3.up / 2;

            pickup.gameObject.SetActive(true);
        }

        // Destroy instance
        Destroy(gameObject);
    }

    #endregion
}
