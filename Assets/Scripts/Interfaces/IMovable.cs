using UnityEngine;

public delegate void Move(Vector2 inDirection);

public interface IMovable
{
    event Move OnMove;

    float MovementSpeed { get; }

    void Init(float inSpeed);
    void Move(Vector2 inDirection);
    void OnMovement(Vector2 inDirection);
}