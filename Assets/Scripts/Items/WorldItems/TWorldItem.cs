using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class TWorldItem : MonoBehaviour 
{
    #region Public properties

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

    public TItemWorldObject ReferenceItem { get; set; }

    public TMap PlacingLayer { get; set; }

    public TWorldGroupID GroupID { get { return m_GroupID; } }

    #endregion

    #region Serialize variables

    [SerializeField] private int m_MaxHitPoints;

    [SerializeField] private TWorldGroupID m_GroupID;

    [SerializeField] private TItemWorldObject m_DefaultReferenceItem;

    private int m_HitPoints;

    #endregion

    #region MonoBehaviour cycle

    protected virtual void Awake()
    {
        // Cache components references
        TransformComponent = transform;
        if(!SpriteRendererComponent) SpriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        m_HitPoints = m_MaxHitPoints;

        if (!ReferenceItem) ReferenceItem = m_DefaultReferenceItem;
    }

    #endregion

    #region Public methods

    public void TakeDamage(int inDamageDealt = 1)
    {
        m_HitPoints -= inDamageDealt;

        if (m_HitPoints <= 0)
            Destroy();
    }

    #endregion

    #region Private methods

    public void Destroy()
    {
        if (ReferenceItem)
        {
            TItemPickup pickup = ObjectPooler.SharedInstance.GetPooledObject("Pickup").GetComponent<TItemPickup>();

            pickup.LoadItem(new TItemQuantity(ReferenceItem));

            pickup.TransformComponent.position = TransformComponent.position + TTilemapManager.SharedInstance.CellSize.x * ReferenceItem.Size.x * Vector3.right / 2
                                                                             + TTilemapManager.SharedInstance.CellSize.y * ReferenceItem.Size.y * Vector3.up / 2;

            pickup.gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }

    #endregion
}
