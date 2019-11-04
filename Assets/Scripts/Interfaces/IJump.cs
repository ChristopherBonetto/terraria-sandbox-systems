using UnityEngine;

public interface IJump
{
    Rigidbody2D Rb { get; }
    float Force { get; }

    void Init(float inForce);
    void Jump(Vector2 inDirection);
    void OnJumpDecision(Vector2 inDirection);
}