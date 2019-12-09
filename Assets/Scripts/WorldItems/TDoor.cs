using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TDoor : TWorldItem, IInteractable
{
    /// <summary>
    /// Private struct represnting a Door state (e.g. Open/Closed)
    /// </summary>
    [System.Serializable]
    private struct TDoorState
    {
        /// <summary>
        /// GameObject layer.
        /// </summary>
        public string Layer;

        /// <summary>
        /// Rendered Sprite.
        /// </summary>
        public Sprite VisualSprite;

        /// <summary>
        /// Collider offset from pivot.
        /// </summary>
        public Vector2 ColliderOffset;

        /// <summary>
        /// Collider size.
        /// </summary>
        public Vector2 ColliderSize;

        /// <summary>
        /// Sprite offset from pivot.
        /// </summary>
        public Vector2 SpriteOffset;
    }



    #region Serialized variables

    [Header("Door states")]

    [SerializeField] private TDoorState m_OpenState;
    [SerializeField] private TDoorState m_ClosedState;

    [Space]

    [SerializeField] private bool m_StartOpen = false;

    #endregion

    #region Private variables

    /// <summary>
    /// Collider component attached to the GameObject.
    /// </summary>
    private BoxCollider2D m_ColliderComponent;

    /// <summary>
    /// Transform component of the Door's sprite.
    /// </summary>
    private Transform m_SpriteTransformComponent;

    #endregion

    #region MonoBehaviour cycle

    protected override void Awake()
    {
        base.Awake();

        // Cache components
        m_ColliderComponent = GetComponentInChildren<BoxCollider2D>();
        m_SpriteTransformComponent = SpriteRendererComponent.transform; 
    }

    protected override void Start()
    {
        base.Start();

        // Init state
        LoadState(m_StartOpen ? m_OpenState : m_ClosedState);
    }

    #endregion

    /// <summary>
    /// Executes the Door interaction, opening/closing it.
    /// </summary>
    public void Interact()
    {
        // Set collider trigger setting
        m_ColliderComponent.isTrigger = !m_ColliderComponent.isTrigger;

        // Load state
        LoadState(m_ColliderComponent.isTrigger ? m_OpenState : m_ClosedState);
    }

    /// <summary>
    /// Loads the specified Door state.
    /// </summary>
    /// <param name="inState"></param>
    private void LoadState(TDoorState inState)
    {
        m_ColliderComponent.offset = inState.ColliderOffset;
        m_ColliderComponent.size = inState.ColliderSize;
        gameObject.layer = LayerMask.NameToLayer(inState.Layer);
        SpriteRendererComponent.sprite = inState.VisualSprite;
        m_SpriteTransformComponent.localPosition = inState.SpriteOffset;
    }
}
