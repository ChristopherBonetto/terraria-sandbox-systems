using UnityEngine;

public delegate void JumpEvent(Vector2 inDirection);

public interface IJump
{
    /// <summary>
    /// Invoke this event when someone decide to jump.
    /// </summary>
    event JumpEvent OnJumpEvent;


    /// <summary>
    /// Rb of the gameObject.
    /// </summary>
    Rigidbody2D Rb { get; }

    /// <summary>
    /// Force used to jump. doesn't require the model
    /// </summary>
    float Force { get; }


    /// <summary>
    /// initialize the rigidbody and the jump force.
    /// </summary>
    void Init(Rigidbody2D rb, float inForce);

    /// <summary>
    /// Make the gameObject jump in a direction.
    /// </summary>
    void Jump(Vector2 inDirection);

    /// <summary>
    /// Invoke the OnJumpEvent.
    /// </summary>
    void OnJumpDecision(Vector2 inDirection);
}