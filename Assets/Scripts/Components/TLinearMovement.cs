using UnityEngine;
using System.Collections;

public class TLinearMovement : MonoBehaviour, IMovable
{
    public event MoveEvent OnMoveEvent;

    [Header("Movement variables")]
    [SerializeField]
    private float m_MovementSpeed;
    public float MovementSpeed { get { return m_MovementSpeed; } }

    [Header("Movement Fix")]
    [SerializeField] [Tooltip("If player detect this mask, he can't walk along that direction")]
    private LayerMask m_ObstacleMask;
    public LayerMask ObstacleMasl { get { return m_ObstacleMask; } }

    [SerializeField]
    private float m_DistanceToDetection; //@TEMP

    private Vector3 m_LastDirection;
    public Vector3 LastDirection { get { return m_LastDirection; } }

    private void OnEnable()
    {
        OnMoveEvent += Move;
    }

    private void OnDisable()
    {
        OnMoveEvent -= Move;
    }

    public void Init(float inSpeed)
    {
        m_MovementSpeed = inSpeed;
    }

    public void Move(Vector2 inDirection)
    {
        // Store last direction
        m_LastDirection = inDirection.normalized;

        // Check if it's colliding with walls
        if (!Physics2D.Raycast(transform.position, inDirection, m_DistanceToDetection, m_ObstacleMask))
            transform.position += (Vector3) (LastDirection * MovementSpeed * Time.deltaTime); 
    }

    public void OnMovement(Vector2 inDirection)
    {
        OnMoveEvent(inDirection);
    }
}
