using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager SharedInstance { get; private set; }

    #region Public properties

    /// <summary>
    /// User Player instance.
    /// </summary>
    public TPlayerController PlayerInstance { get; private set; }

    #endregion

    #region Serialized variables

    [Header("Player")]
    [SerializeField] private TPlayerController m_PlayerPrefab;

    [Header("Respawn")]

    /// <summary>
    /// Default point at which the Player respawns.
    /// </summary>
    [SerializeField] private Transform m_DefaultSpawnPoint;

    /// <summary>
    /// Delay before respawn execution.
    /// </summary>
    [SerializeField] private float m_RespawnDelay;

    [Space]

    /// <summary>
    /// Message shown on Spawn Point being set.
    /// </summary>
    [SerializeField, TextArea] private string m_SpawnPointSetMessage;

    /// <summary>
    /// Duration of Spawn Point message.
    /// </summary>
    [SerializeField] private float m_SpawnPointSetMessageDuration;

    #endregion

    #region Private variables

    private TSpawnPoint m_SpawnPoint;

    #endregion


    #region MonoBehaviour cycle

    private void Awake()
    {
        // Set singleton instance
        SharedInstance = this;

        // Instantiate Player instance
        PlayerInstance = Instantiate(m_PlayerPrefab, m_DefaultSpawnPoint.position, Quaternion.identity);
    }

    private void Start()
    {
        // Trigger Spawn event
        TEventManager.TriggerEvent(TEventID.OnPlayerSpawned, PlayerInstance);
    }


    private void OnEnable()
    {
        // Subscribe to events
        TEventManager.SubscribeTo(TEventID.OnPlayerDied, StartRespawn);
        TEventManager.SubscribeTo<TSpawnPoint>(TEventID.OnSpawnPointSelected, SetSpawnPoint);
        TEventManager.SubscribeTo<TSpawnPoint>(TEventID.OnSpawnPointRemoved, RemoveSpawnPoint);
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        TEventManager.UnsubscribeFrom(TEventID.OnPlayerDied, StartRespawn);
        TEventManager.UnsubscribeFrom<TSpawnPoint>(TEventID.OnSpawnPointSelected, SetSpawnPoint);
        TEventManager.UnsubscribeFrom<TSpawnPoint>(TEventID.OnSpawnPointRemoved, RemoveSpawnPoint);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Sets current Spawn Point.
    /// </summary>
    /// <param name="inSpawnPoint"></param>
    public void SetSpawnPoint(TSpawnPoint inSpawnPoint)
    {
        m_SpawnPoint = inSpawnPoint;

        TEventManager.TriggerEvent(TEventID.OnMessageSent, m_SpawnPointSetMessage, m_SpawnPointSetMessageDuration);
    }

    /// <summary>
    /// Removes current Spawn Point.
    /// </summary>
    /// <param name="inSpawnPoint"></param>
    public void RemoveSpawnPoint(TSpawnPoint inSpawnPoint)
    {
        if (inSpawnPoint == m_SpawnPoint) m_SpawnPoint = null;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Invokes Respawn method with a delay.
    /// </summary>
    private void StartRespawn()
    {
        Invoke("Respawn", m_RespawnDelay);
    }

    /// <summary>
    /// Respawn the Player.
    /// </summary>
    private void Respawn()
    {
        // Move player to Spawn Point (default if no Spawn Point is set)
        PlayerInstance.transform.position = m_SpawnPoint ? m_SpawnPoint.SpawnPosition : m_DefaultSpawnPoint.position;

        // Re-init Player
        PlayerInstance.Init();
        PlayerInstance.gameObject.SetActive(true);
    }

    #endregion
}