using UnityEngine;
using System.Collections;

public class TLinearMovement : MonoBehaviour, IMovable
{
    public event Move OnMove;

    [SerializeField]
    private float m_MovementSpeed;
    public float MovementSpeed => m_MovementSpeed;

    [SerializeField]
    private LayerMask m_JumpableMask;
    public LayerMask JumpableMask => m_JumpableMask;

    [SerializeField]
    private float m_DistanceToDetection; //@TEMP

    private Vector3 m_LastDirection;
    public Vector3 LastDirection { get { return m_LastDirection; } }

    private void OnEnable()
    {
        OnMove += Move;
    }

    private void OnDisable()
    {
        OnMove -= Move;
    }

    public void Init(float inSpeed)
    {
        m_MovementSpeed = inSpeed;
    }

    public void Move(Vector2 inDirection)
    {
        m_LastDirection = inDirection.normalized;

        if (!Physics2D.Raycast(transform.position, inDirection, m_DistanceToDetection, m_JumpableMask))
            transform.position += (Vector3) (LastDirection * MovementSpeed * Time.deltaTime); 
    }

    public void OnMovement(Vector2 inDirection)
    {
        OnMove(inDirection);
    }
}
