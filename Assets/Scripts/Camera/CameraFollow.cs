using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static Camera MainCamera { get; private set; }

    public Transform TransformComponent { get; private set; }

    [SerializeField] private Transform m_Target;
    [SerializeField] private Vector3 m_Offset;

    [SerializeField] private bool m_IsSmooth;

    [SerializeField] private float m_MovementSpeed;

    private void Awake()
    {
        MainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var newPosition = m_Target.position + m_Offset;

        if (m_IsSmooth)
            transform.position = Vector3.MoveTowards(transform.position, newPosition, m_MovementSpeed * Time.deltaTime);

        else
            transform.position = newPosition;
    }
}
