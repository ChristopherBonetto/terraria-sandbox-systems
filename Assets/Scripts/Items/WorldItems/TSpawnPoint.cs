using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSpawnPoint : MonoBehaviour, IInteractable
{
    public Transform TransformComponent { get; private set; }

    public Vector3 SpawnPosition { get { return TransformComponent.position + m_SpawnOffset; } }

    [SerializeField] private Vector3 m_SpawnOffset;

    private void Awake()
    {
        TransformComponent = transform;
    }

    private void OnDisable()
    {
        TEventManager.TriggerEvent(TEventID.OnSpawnPointRemoved, this);
    }

    public void Interact()
    {
        TEventManager.TriggerEvent(TEventID.OnSpawnPointSelected, this);
    }
}
