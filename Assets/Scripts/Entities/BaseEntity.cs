using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [Header("Unity components to store")]

    [SerializeField] protected Transform m_Transform;
    [SerializeField] protected Collider2D m_Collider;
    [SerializeField] protected Rigidbody2D m_Rb;

    protected Vector3 m_LocalScale;

    protected virtual void Start()
    {
        m_LocalScale = m_Transform.localScale;
    }
}
