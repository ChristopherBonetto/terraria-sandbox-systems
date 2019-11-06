using UnityEngine;
using System.Collections;

public class LinearMovement : MonoBehaviour, IMovable
{
    [SerializeField] private float m_MovementSpeed;
    public float MovementSpeed => m_MovementSpeed;


    private void OnEnable()
    {
        // subscribe
    }

    private void OnDisable()
    {
        // unsibscribe
    }

    public void Init(float inSpeed)
    {
        m_MovementSpeed = inSpeed;
    }

    public void Move(Vector2 inDirection)
    {
        transform.position += (Vector3) (inDirection.normalized * MovementSpeed * Time.deltaTime); 
    }

    public void OnMovement(Vector2 inDirection)
    {
    }
}
