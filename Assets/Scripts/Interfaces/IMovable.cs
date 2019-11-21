using UnityEngine;

public interface IMovable
{
    /// <summary>
    /// Movement speed. ( from model ) now doesn't require the model
    /// </summary>
    float MovementSpeed { get; }


    /// <summary>
    /// Initialize this speed as the model one.
    /// </summary>
    void Init(float inSpeed);

    /// <summary>
    /// Move the entity in a direction.
    /// </summary>
    void Move(Vector2 inDirection);
}