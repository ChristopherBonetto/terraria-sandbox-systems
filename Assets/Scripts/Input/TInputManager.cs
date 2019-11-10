using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MouseIndex
{
    Left = 0,
    Right = 1,
    Center = 2
}

public class TInputManager : MonoBehaviour
{
    #region Singleton Instance reference

    /// <summary>
    /// Singleton Instance reference.
    /// </summary>
    public static TInputManager SharedInstance { get; private set; }

    #endregion

    #region Input delegates definition

    public delegate void TClickEvent(TClickData inData);
    public delegate void TAxisEvent(float inAxis);
    public delegate void TButtonEvent();

    #endregion

    #region Input events
    
    // Left click
    public event TClickEvent OnLeftClickDown;
    public event TClickEvent OnLeftClickUp;

    // Right click
    public event TClickEvent OnRightClickDown;
    public event TClickEvent OnRightClickUp;

    // Movement
    public event TAxisEvent OnMovementAxis;

    // Jump
    public event TButtonEvent OnJumpDown;
    public event TButtonEvent OnJumpUp;

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Update()
    {
        CheckMouseInput();
        CheckMovementInput();
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Checks for Mouse inputs and invokes associated events.
    /// </summary>
    private void CheckMouseInput()
    {
        #region Left click

            if (Input.GetMouseButtonDown(0))
            OnLeftClickDown?.Invoke(new TClickData(Input.mousePosition));

        else if (Input.GetMouseButtonUp(0))
            OnLeftClickUp?.Invoke(new TClickData(Input.mousePosition));

        #endregion

        #region Right click

        if (Input.GetMouseButtonDown(1))
            OnRightClickDown?.Invoke(new TClickData(Input.mousePosition));

        else if (Input.GetMouseButtonUp(1))
            OnRightClickUp?.Invoke(new TClickData(Input.mousePosition));

        #endregion
    }

    /// <summary>
    /// Checks for Movement inputs and invokes associated events.
    /// </summary>
    private void CheckMovementInput()
    {
        #region Horizontal movement

        float moveAxis = Input.GetAxisRaw(TControls.MovementAxis);

        if (moveAxis != 0)
            OnMovementAxis?.Invoke(moveAxis);

        #endregion

        #region Jump

        if (Input.GetButtonDown(TControls.Jump))
            OnJumpDown?.Invoke();

        else if (Input.GetButtonUp(TControls.Jump))
            OnJumpUp?.Invoke();

        #endregion
    }

    #endregion

}

/// <summary>
/// Struct containing information about a Click event.
/// </summary>
public struct TClickData
{
    public Vector3 ScreenPosition;
    public Vector3 WorldPosition;
    public Vector3Int GridPosition;

    public TClickData(Vector3 inMousePosition)
    {
        ScreenPosition = inMousePosition;
        WorldPosition = CameraFollow.MainCamera.ScreenToWorldPoint(inMousePosition);
        GridPosition = TTilemapManager.SharedInstance.WorldToGridPosition(WorldPosition);
    }

    public override string ToString()
    {
        return "[Screen: " + ScreenPosition + " | World: " + WorldPosition + " | Grid: " + GridPosition + "]";
    }

}

/// <summary>
/// Class containing constant string identification for the game's controls.
/// </summary>
public static class TControls
{
    public static readonly string Jump = "Jump";
    public static readonly string MovementAxis = "Horizontal";
}
