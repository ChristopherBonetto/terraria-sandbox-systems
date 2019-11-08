using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpComponent : MonoBehaviour, IJump
{
    private Rigidbody2D m_Rb;
    [SerializeField] private float m_Force;

    public event Jump OnJump;

    public Rigidbody2D Rb => m_Rb;
    public float Force => m_Force;

    public void Init(Rigidbody2D rb)
    {
        m_Rb = rb;
    }

    public void Init(Rigidbody2D rb, float inForce)
    {
        m_Rb = rb;
        m_Force = inForce;
    }

    private void OnEnable()
    {
        OnJump += Jump;
    }

    private void Update()
    {
        if (Rb.velocity.y < 0)
        {
            Rb.velocity += Vector2.up * (Physics2D.gravity.y + 9.5f);
        }
    }

    private void OnDisable()
    {
        OnJump -= Jump;
    }

    public void Jump(Vector2 inDirection)
    {
        Rb.AddForce(inDirection.normalized * m_Force);
    }

    public void OnJumpDecision(Vector2 inDirection)
    {
        OnJump(inDirection);
    }
}
