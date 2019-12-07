using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static Camera MainCamera { get; private set; }

    public Transform TransformComponent { get; private set; }

    [SerializeField] private Vector3 m_Offset;

    [SerializeField] private bool m_IsSmooth;

    [SerializeField] private float m_MovementSpeed;

    private Transform m_Target;

    private void Awake()
    {
        MainCamera = GetComponent<Camera>();
        TransformComponent = transform;
    }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TPlayerController>(TEventID.OnPlayerSpawned, SetPlayerTarget);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TPlayerController>(TEventID.OnPlayerSpawned, SetPlayerTarget);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var newPosition = m_Target.position + m_Offset;

        if (m_IsSmooth)
            TransformComponent.position = Vector3.MoveTowards(TransformComponent.position, newPosition, m_MovementSpeed * Time.deltaTime);

        else
            TransformComponent.position = newPosition;
    }

    public void SetPlayerTarget(TPlayerController inPlayer)
    {
        m_Target = inPlayer.transform;
        TransformComponent.position = m_Target.position + m_Offset;
    }
}
