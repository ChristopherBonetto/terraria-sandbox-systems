using UnityEngine;

public delegate void MoveEvent(Vector2 inDirection);

public interface IMovable
{
    /// <summary>
    /// Invoke this event when someone decide to move.
    /// </summary>
    event MoveEvent OnMoveEvent;


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

    /// <summary>
    /// Invoke the OnMove event.
    /// </summary>
    void OnMovement(Vector2 inDirection);
}