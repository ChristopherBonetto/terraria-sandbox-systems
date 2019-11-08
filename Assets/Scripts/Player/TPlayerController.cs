using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TJumpComponent), typeof(TDefenseComponent), typeof(TLinearMovement))]
public class TPlayerController : MonoBehaviour
{
    #region Private
    private IMovable m_MovementComponent;
    private IDefend m_DefenseComponent;
    private IJump m_JumpComponent;
    private Rigidbody2D m_Rb;

    [SerializeField] private float m_DistanceToDetection; //@TEMP
    [SerializeField] private LayerMask m_JumpableMask;
    [SerializeField] private float m_MaxHealth;
    #endregion

    #region Properties
    public IMovable MovementComponent
    {
        get
        {
            if (m_MovementComponent == null)
                m_MovementComponent = GetComponent<IMovable>();
            return m_MovementComponent;
        }
    }
    public IDefend DefenseComponent
    {
        get
        {
            if (m_DefenseComponent == null)
                m_DefenseComponent = GetComponent<IDefend>();
            return m_DefenseComponent;
        }
    }
    public IJump JumpComponent
    {
        get
        {
            if (m_JumpComponent == null)
                m_JumpComponent = GetComponent<IJump>();
            return m_JumpComponent;
        }
    }
    public Rigidbody2D Rb
    {
        get
        {
            if (m_Rb == null)
                m_Rb = GetComponent<Rigidbody2D>();
            return m_Rb;
        }
    }

    public LayerMask JumpableMask => m_JumpableMask;
    public float MaxHealth => m_MaxHealth;
    #endregion


    private void OnEnable()
    {
        // subscribe
    }

    private void Start()
    {
        JumpComponent.Init(Rb);
    }

    private void Update()
    {
        // Movement
        float direction = Input.GetAxisRaw("Horizontal");

        // @TEMP
        Debug.DrawRay(transform.position, Vector2.right * direction * m_DistanceToDetection, Color.blue);

        if (direction != 0)
        {
            if (!Physics2D.Raycast(transform.position, Vector2.right * direction, m_DistanceToDetection, JumpableMask))
            {
                // @TODO : delete this and add right and left animation.
                transform.localScale = new Vector3(1 * direction, 1, 1); 

                MovementComponent.OnMovement(Vector2.right * direction);
            }
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics2D.OverlapCircle(transform.position, m_DistanceToDetection, JumpableMask, 0))
                JumpComponent.OnJumpDecision(Vector2.up);
        }
        else if (!Input.GetKey(KeyCode.Space) && JumpComponent.Rb.velocity.y > 0)
        {
            JumpComponent.Rb.velocity += Vector2.up * (Physics2D.gravity.y + 9.5f);
        }
    }

    private void OnDisable()
    {
        // unsibscribe
    }
}
