using UnityEngine;

public delegate void Jump(Vector2 inDirection);

public interface IJump
{
    event Jump OnJump;

    Rigidbody2D Rb { get; }
    float Force { get; }

    void Init(Rigidbody2D rb);
    void Init(Rigidbody2D rb, float inForce);
    void Jump(Vector2 inDirection);
    void OnJumpDecision(Vector2 inDirection);
}