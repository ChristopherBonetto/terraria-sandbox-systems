using UnityEngine;
using System.Collections;

[RequireComponent(typeof(JumpComponent), typeof(DefenseComponent), typeof(LinearMovement))]
public class PlayerController : MonoBehaviour
{
    private IMovable m_MovementComponent;
    private IDefend m_DefenseComponent;
    private IJump m_JumpComponent;

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


    private void OnEnable()
    {
        // subscribe
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        if (direction != 0)
            MovementComponent.Move(Vector2.right * direction);

        if (Input.GetKeyDown(KeyCode.Space))
            JumpComponent.Jump(Vector2.up);

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
