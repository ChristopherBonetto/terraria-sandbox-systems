using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpComponent : MonoBehaviour, IJump
{
    private Rigidbody2D m_Rb;
    [SerializeField] private float m_Force;

    public Rigidbody2D Rb
    {
        get
        {
            if (m_Rb == null)
                m_Rb = GetComponent<Rigidbody2D>();
            return m_Rb;
        }
    }
    public float Force => m_Force;


    public void Init(float inForce)
    {
        m_Force = inForce;
    }

    private void Update()
    {
        if (Rb.velocity.y < 0)
        {
            Rb.velocity += Vector2.up * (Physics2D.gravity.y + 9.5f);
        }
    }

    public void Jump(Vector2 inDirection)
    {
        Rb.AddForce(inDirection.normalized * m_Force);
    }

    public void OnJumpDecision(Vector2 inDirection)
    {
        // Call event
    }
}
