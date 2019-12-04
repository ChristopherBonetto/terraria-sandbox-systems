using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TDoor : TWorldItem, IInteractable
{
    [System.Serializable]
    private struct TDoorState
    {
        public string Layer;
        public Sprite VisualSprite;
        public Vector2 ColliderOffset;
        public Vector2 ColliderSize;
    }

    [SerializeField] private TDoorState m_OpenState;
    [SerializeField] private TDoorState m_ClosedState;

    private BoxCollider2D m_ColliderComponent;

    protected override void Awake()
    {
        base.Awake();

        m_ColliderComponent = GetComponentInChildren<BoxCollider2D>();
    }

    protected override void Start()
    {
        LoadState(m_ClosedState);
    }

    public void Interact()
    {
        m_ColliderComponent.isTrigger = !m_ColliderComponent.isTrigger;

        LoadState(m_ColliderComponent.isTrigger ? m_OpenState : m_ClosedState);
    }

    private void LoadState(TDoorState inState)
    {
        m_ColliderComponent.offset = inState.ColliderOffset;
        m_ColliderComponent.size = inState.ColliderSize;
        gameObject.layer = LayerMask.NameToLayer(inState.Layer);
        SpriteRendererComponent.sprite = inState.VisualSprite;

    }
}
