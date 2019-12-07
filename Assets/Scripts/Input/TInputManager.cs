using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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


    [SerializeField] private KeyCode m_keyToOpenInventory;

    private bool inventoryBool = false;

    private TPlayerController m_UserPlayer;
    

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
        CheckScroll();
        OpenCloseInventory();
    }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TPlayerController>(TEventID.OnPlayerSpawned, SetUserPlayer);
        StartCoroutine(WaitToStart());
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TPlayerController>(TEventID.OnPlayerSpawned, SetUserPlayer);
        StopAllCoroutines();
    }

    #endregion

    #region Private methods

    private void SetUserPlayer(TPlayerController inPlayer)
    {
        m_UserPlayer = inPlayer;
    }

    /// <summary>
    /// Checks for Mouse inputs and invokes associated events.
    /// </summary>
    private void CheckMouseInput()
    {
        #region Left click

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            m_UserPlayer.UseEquippedItem(new TPointerData(Input.mousePosition));

        else if (Input.GetMouseButtonUp(1) && !EventSystem.current.IsPointerOverGameObject())
            m_UserPlayer.Act(new TPointerData(Input.mousePosition));

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

    #region Inventory System
    private IEnumerator WaitToStart()
    {
        yield return new WaitForFixedUpdate();
        TriggerInventoryEvent();
    }

    public void OpenCloseInventory()
    {
        if (Input.GetKeyDown(m_keyToOpenInventory))
        {
            TriggerInventoryEvent();
        }
    }

    private void TriggerInventoryEvent()
    {
        TEventManager.TriggerEvent(TEventID.OnInventoryOpen, inventoryBool);
        inventoryBool = !inventoryBool;
    }
    #endregion

    #region Keyboard Input System
    private void CheckKeyboardNumber()
    {
        if (inventoryBool && Input.anyKeyDown)
        {
            int? tempNumber = ReturnKeyboardNumber();

            if (tempNumber.HasValue)
            {
                int effectiveNumber = (int)tempNumber - 1;

                m_UserPlayer.PlayerInventoryComponent.InventorySlots[effectiveNumber].SelectSlot();
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
    #endregion

    #region Crafting Scrollbar System
    public void CheckScroll()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        
        if(scrollValue != 0)
        {            
            TUIManager.SharedInstance.ScrollCraftingBar(scrollValue);
        }
    }
    #endregion


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




