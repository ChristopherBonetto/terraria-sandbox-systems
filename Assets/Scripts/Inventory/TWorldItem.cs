using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class TWorldItem : MonoBehaviour 
{
    #region Public properties
    /// <summary>
    /// Cached reference to the attached Transform component.
    /// </summary>
    public Transform TransformComponent { get; private set; }

    /// <summary>
    /// Cached reference to the attached Collider2D component.
    /// </summary>
    public Collider2D ColliderComponent { get; private set; }

    /// <summary>
    /// Cached reference to the Renderer component attached to the GameObject or one of its children.
    /// </summary>
    public Renderer RendererComponent { get; private set; }

    public TItem ReferenceItem { get; set; }

    #endregion

    #region Serialize variables
    
    [SerializeField] private int m_HitPoints;

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        // Cache components references
        TransformComponent = transform;
        ColliderComponent = GetComponent<Collider2D>();
        RendererComponent = GetComponentInChildren<Renderer>();
    }

    #endregion

    #region Private methods

    public void Destroy()
    {
        if (ReferenceItem)
        {
            TItemPickup pickup = ObjectPooler.SharedInstance.GetPooledObject("Pickup").GetComponent<TItemPickup>();

            pickup.LoadItem(new TItemQuantity(ReferenceItem));

            pickup.TransformComponent.position = TransformComponent.position;

            pickup.gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }

    #endregion
}
