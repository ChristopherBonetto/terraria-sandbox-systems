using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSpawnPoint : MonoBehaviour, IInteractable
{
    #region Public properties

    /// <summary>
    /// Transform component attached to the GameObject.
    /// </summary>
    public Transform TransformComponent { get; private set; }

    /// <summary>
    /// Actual spawn position.
    /// </summary>
    public Vector3 SpawnPosition { get { return TransformComponent.position + m_SpawnOffset; } }

    #endregion

    #region Serialized variables

    /// <summary>
    /// Spatial offset from the Transform's position determining the actual Spawn position.
    /// </summary>
    [SerializeField] private Vector3 m_SpawnOffset;

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        // Cache Transform component
        TransformComponent = transform;
    }

    private void OnDisable()
    {
        // When disabled, the spawn point is no more available
        TEventManager.TriggerEvent(TEventID.OnSpawnPointRemoved, this);
    }

    #endregion

    /// <summary>
    /// Executes the Spawn Point interaction, setting the Spawn Point to this one.
    /// </summary>
    public void Interact()
    {
        TEventManager.TriggerEvent(TEventID.OnSpawnPointSelected, this);
    }
}
