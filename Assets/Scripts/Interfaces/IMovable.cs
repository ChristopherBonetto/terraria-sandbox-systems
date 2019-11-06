using UnityEngine;

public interface IMovable
{
    float MovementSpeed { get; }

    void Init(float inSpeed);
    void Move(Vector2 inDirection);
    void OnMovement(Vector2 inDirection);
}