using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TItemPickup : MonoBehaviour
{
    #region Public properties
    /// <summary>
    /// Transform component attached to the GameObject.
    /// </summary>
    public Transform TransformComponent { get; private set; }

    #endregion

    #region  Serialized variables
    /// <summary>
    /// Item contained inside of the pickup.
    /// </summary>
    [SerializeField] private TItemQuantity m_ContainedItem;

    #endregion

    #region Private variables

    /// <summary>
    /// Sprite Renderer component attached to the GameObject.
    /// </summary>
    private SpriteRenderer m_SpriteRendererComponent;

    #endregion


    #region MonoBehaviour cycle

    private void Awake()
    {
        // Cache component references
        TransformComponent = transform;
        m_SpriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (m_ContainedItem.Item != null)
            m_SpriteRendererComponent.sprite = m_ContainedItem.Item.ItemSprite;
    }

    #endregion

    #region Collision management

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If tirggered by player, pick up
        TPlayerController pc = collision.GetComponent<TPlayerController>();

        if (pc)
        {
            TInventory.Instance.CollectItem(m_ContainedItem);
            gameObject.SetActive(false);
        }
    }

    #endregion


    #region Public methods
    /// <summary>
    /// Loads the specified Item as contained in the pick up.
    /// </summary>
    /// <param name="inItemQuantity"></param>
    public void LoadItem(TItemQuantity inItemQuantity)
    {
        // Set contained Item
        m_ContainedItem = inItemQuantity;
        // Set Sprite
        m_SpriteRendererComponent.sprite = inItemQuantity.Item.ItemSprite;
    }

    #endregion
}
