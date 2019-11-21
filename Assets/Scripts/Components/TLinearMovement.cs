using UnityEngine;
using System.Collections;

public class TLinearMovement : MonoBehaviour, IMovable
{
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

    private Vector3 m_Scale;


    public void Init(float inSpeed)
    {
        m_MovementSpeed = inSpeed;
    }

    private void Start()
    {
        m_Scale = transform.localScale;
    }

    public void Move(Vector2 inDirection)
    {
        // Store last direction
        m_LastDirection = inDirection.normalized;
        transform.localScale = new Vector3(inDirection.x * m_Scale.x, m_Scale.y);

        // Check if it's colliding with walls
        if (!Physics2D.Raycast(transform.position, inDirection, m_DistanceToDetection, m_ObstacleMask))
            transform.position += (Vector3) (LastDirection * MovementSpeed * Time.deltaTime); 
    }
}
