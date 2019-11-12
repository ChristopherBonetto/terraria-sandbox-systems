using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class TJumpComponent : MonoBehaviour, IJump
{
    public event Jump OnJump;

    [SerializeField]
    private float m_Force;
    public float Force => m_Force;

    [SerializeField]
    private LayerMask m_JumpableMask;
    public LayerMask JumpableMask => m_JumpableMask;

    [SerializeField]
    private Transform m_Legs;

    [SerializeField]
    private float m_DistanceToDetection; //@TEMP

    private Collider2D m_Col;

    private Rigidbody2D m_Rb;
    public Rigidbody2D Rb => m_Rb;


    private void Awake()
    {
        m_Col = GetComponent<Collider2D>();
        m_Rb = GetComponent<Rigidbody2D>();
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

    public void Init(Rigidbody2D rb, float inForce)
    {
        m_Rb = rb;
        m_Force = inForce;
    }

    public void Jump(Vector2 inDirection)
    {
            // Check bottom right point
        if (Physics2D.Raycast(m_Legs.position, Vector2.down + Vector2.right * m_Col.bounds.extents.x, m_DistanceToDetection, JumpableMask) ||
            // Check bottom left point
            Physics2D.Raycast(m_Legs.position, Vector2.down + Vector2.right * -m_Col.bounds.extents.x, m_DistanceToDetection, JumpableMask))
            Rb.AddForce(inDirection.normalized * m_Force);
    }

    public void OnJumpDecision(Vector2 inDirection)
    {
        OnJump(inDirection);
    }
}
