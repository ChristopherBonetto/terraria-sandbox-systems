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

    [SerializeField] private TPlayerController m_UserPlayer;

    #region MonoBehaviour cycle

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Update()
    {
        CheckMouseInput();
        CheckMovementInput();
        CheckKeyboardNumber();
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

        if (Input.GetMouseButtonUp(0))
            m_UserPlayer.UseEquippedItem(new TPointerData(Input.mousePosition));

        #endregion
    }

    /// <summary>
    /// Checks for Movement inputs and invokes associated events.
    /// </summary>
    private void CheckMovementInput()
    {
        #region Horizontal movement

        m_UserPlayer.Move(Input.GetAxisRaw(TControls.MovementAxis));
        
        #endregion

        #region Jump


        #endregion
    }

    private void CheckKeyboardNumber()
    {
        if (Input.anyKeyDown)
        {
            int? tempNumber = ReturnKeyboardNumber();

            if (tempNumber.HasValue)
            {
                int effectiveNumber = (int)tempNumber - 1;

                //TInventory.Instance.InventorySlots[effectiveNumber].SelectThisSlotForEvent();

            }
        }
    }

    private int? ReturnKeyboardNumber()
    {        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            return 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            return 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            return 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            return 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            return 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            return 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            return 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            return 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            return 9;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            return 10;
        }
        else
        {
            return null;
        }
        
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
            if (IsPointerTrackingBeingRequested())
            {
                // Get current cell the pointer is hovering on
                newPointerCell = TTilemapManager.SharedInstance.WorldToGridPosition(CameraFollow.MainCamera.ScreenToWorldPoint(Input.mousePosition));

                // If it's different from the previous one, update it and raise event
                if (newPointerCell != oldPointerCell)
                {
                    oldPointerCell = newPointerCell;
                    TEventManager.TriggerEvent(TEventID.OnPointerMovedOnGrid, new TPointerData(Input.mousePosition));
                }
                
                yield return null;
            }
            else
            {
                yield return new WaitUntil(IsPointerTrackingBeingRequested);
            }
        }
    }

    /// <summary>
    /// Checks if anyone is requesting pointer tracking.
    /// </summary>
    /// <returns>True if pointer tracking is requested.</returns>
    private bool IsPointerTrackingBeingRequested()
    {
        return TEventManager.Exists(TEventID.OnPointerMovedOnGrid);
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
