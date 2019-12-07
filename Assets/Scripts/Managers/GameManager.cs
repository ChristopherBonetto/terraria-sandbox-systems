using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager SharedInstance { get; private set; }

    public TPlayerController PlayerInstance { get; private set; }

    [SerializeField] private Transform m_DefaultSpawnPoint;

    [SerializeField] private TPlayerController m_PlayerPrefab;

    [SerializeField] private float m_RespawnDelay;
    private TSpawnPoint m_SpawnPoint;

    private void Awake()
    {
        SharedInstance = this;

        PlayerInstance = Instantiate(m_PlayerPrefab, m_DefaultSpawnPoint.position, Quaternion.identity);
    }

    private void Start()
    {
        TEventManager.TriggerEvent(TEventID.OnPlayerSpawned, PlayerInstance);
    }


    private void OnEnable()
    {
        TEventManager.SubscribeTo(TEventID.OnPlayerDied, StartRespawn);
        TEventManager.SubscribeTo<TSpawnPoint>(TEventID.OnSpawnPointSelected, SetSpawnPoint);
        TEventManager.SubscribeTo<TSpawnPoint>(TEventID.OnSpawnPointRemoved, RemoveSpawnPoint);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom(TEventID.OnPlayerDied, StartRespawn);
        TEventManager.UnsubscribeFrom<TSpawnPoint>(TEventID.OnSpawnPointSelected, SetSpawnPoint);
        TEventManager.UnsubscribeFrom<TSpawnPoint>(TEventID.OnSpawnPointRemoved, RemoveSpawnPoint);
    }

    public void SetSpawnPoint(TSpawnPoint inSpawnPoint)
    {
        m_SpawnPoint = inSpawnPoint;
    }

    public void RemoveSpawnPoint(TSpawnPoint inSpawnPoint)
    {
        if (inSpawnPoint == m_SpawnPoint) m_SpawnPoint = null;
    }

    private void StartRespawn()
    {
        Invoke("Respawn", m_RespawnDelay);
    }

    private void Respawn()
    {
        PlayerInstance.transform.position = m_SpawnPoint ? m_SpawnPoint.SpawnPosition : m_DefaultSpawnPoint.position;
        PlayerInstance.Init();
        PlayerInstance.gameObject.SetActive(true);
    }

    
}
