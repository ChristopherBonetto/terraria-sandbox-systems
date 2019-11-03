using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_Target;

    [SerializeField] private bool m_IsSmooth;

    [SerializeField] private float m_MovementSpeed;

    // Update is called once per frame
    void LateUpdate()
    {
        var newPosition = new Vector3(m_Target.transform.position.x, m_Target.transform.position.y, transform.position.z);

        if (m_IsSmooth)
            transform.position = Vector3.MoveTowards(transform.position, newPosition, m_MovementSpeed * Time.deltaTime);

        else
            transform.position = newPosition;
    }
}
