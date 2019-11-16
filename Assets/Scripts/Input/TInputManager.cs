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

    public delegate void TPointerEvent(TPointerData inData);
    public delegate void TAxisEvent(float inAxis);
    public delegate void TButtonEvent();

    #endregion

    #region Input events
    
    // Left click
    public event TPointerEvent OnLeftClickDown;
    public event TPointerEvent OnLeftClickUp;

    // Right click
    public event TPointerEvent OnRightClickDown;
    public event TPointerEvent OnRightClickUp;

    // Pointer movement
    public event TPointerEvent OnPointerMovedOnGrid;

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

    private void OnEnable()
    {
        StartCoroutine(PointerTrackerCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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
            OnLeftClickDown?.Invoke(new TPointerData(Input.mousePosition));

        else if (Input.GetMouseButtonUp(0))
            OnLeftClickUp?.Invoke(new TPointerData(Input.mousePosition));

        #endregion

        #region Right click

        if (Input.GetMouseButtonDown(1))
            OnRightClickDown?.Invoke(new TPointerData(Input.mousePosition));

        else if (Input.GetMouseButtonUp(1))
            OnRightClickUp?.Invoke(new TPointerData(Input.mousePosition));

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


    /// <summary>
    /// Checks if the pointer moved from a grid cell to another and raises an event when it happens. The coroutine runs only if there is at least one subscriber to the event.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PointerTrackerCoroutine()
    {
        // Wait for subscribers
        yield return new WaitUntil(IsPointerTrackingBeingRequested);

        // On first frame, save cell
        Vector3Int oldPointerCell = TTilemapManager.SharedInstance.WorldToGridPosition(CameraFollow.MainCamera.ScreenToWorldPoint(Input.mousePosition));

        yield return null;

        // From second frame on, perform the check when running
        Vector3Int newPointerCell;

        while (Application.isPlaying)
        {
            // If there no subscribers, wait for new subscribers
            if (OnPointerMovedOnGrid == null)
            {
                yield return new WaitUntil(IsPointerTrackingBeingRequested);
            }
            else
            {
                // Get current cell the pointer is hovering on
                newPointerCell = TTilemapManager.SharedInstance.WorldToGridPosition(CameraFollow.MainCamera.ScreenToWorldPoint(Input.mousePosition));

                // If it's different from the previous one, update it and raise event
                if (newPointerCell != oldPointerCell)
                {
                    oldPointerCell = newPointerCell;
                    OnPointerMovedOnGrid?.Invoke(new TPointerData(Input.mousePosition));
                }
                
                yield return null;
            }
        }
    }

    /// <summary>
    /// Checks if anyone is requesting pointer tracking.
    /// </summary>
    /// <returns>True if pointer tracking is requested.</returns>
    private bool IsPointerTrackingBeingRequested()
    {
        return OnPointerMovedOnGrid != null;
    }

    #endregion

}

/// <summary>
/// Struct containing information about a Click event.
/// </summary>
public struct TPointerData
{
    public Vector3 ScreenPosition;
    public Vector3 WorldPosition;
    public Vector3Int GridPosition;

    public TPointerData(Vector3 inMousePosition)
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
