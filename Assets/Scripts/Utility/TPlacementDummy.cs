using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPlacementDummy : MonoBehaviour
{
    private Transform m_TransformComponent;
    private SpriteRenderer m_SpriteRendererComponent;
    private Vector3 m_Offset;

    private bool m_IsActive;

    private void Awake()
    {
        m_TransformComponent = transform;
        m_SpriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
        m_SpriteRendererComponent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemSelected, OnItemSelected);
        TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemDeselected, OnItemDeselected);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemSelected, OnItemSelected);
        TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemDeselected, OnItemDeselected);
    }

    private void OnItemSelected(TInventorySlot inSlot)
    {
        if (inSlot.ItemInSlot.Item is TItemWorldObject itemWorldObject)
        {
            m_Offset.x = itemWorldObject.Prefab.Size.x * TTilemapManager.SharedInstance.CellSize.x / 2;
            m_Offset.y = itemWorldObject.Prefab.Size.y * TTilemapManager.SharedInstance.CellSize.y / 2;

            m_SpriteRendererComponent.sprite = itemWorldObject.Prefab.ItemSprite;

            if (!m_IsActive)
            {
                m_IsActive = true;
                StartCoroutine("PointerTrackerCoroutine");
                m_SpriteRendererComponent.gameObject.SetActive(true);
            }
        }
    }

    private void OnItemDeselected(TInventorySlot inSlot)
    {
        if (m_IsActive)
        {
            m_IsActive = false;
            StopCoroutine("PointerTrackerCoroutine");
            m_SpriteRendererComponent.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if the pointer moved from a grid cell to another and raises an event when it happens. The coroutine runs only if there is at least one subscriber to the event.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PointerTrackerCoroutine()
    {
        // On first frame, save cell
        Vector3Int oldPointerCell = TTilemapManager.SharedInstance.WorldToGridPosition(CameraFollow.MainCamera.ScreenToWorldPoint(Input.mousePosition));

        yield return null;

        // From second frame on, perform the check when running
        Vector3Int newPointerCell;

        while (Application.isPlaying)
        {
            // Get current cell the pointer is hovering on
            newPointerCell = TTilemapManager.SharedInstance.WorldToGridPosition(CameraFollow.MainCamera.ScreenToWorldPoint(Input.mousePosition));

            // If it's different from the previous one, update it and raise event
            if (newPointerCell != oldPointerCell)
            {
                oldPointerCell = newPointerCell;

                if (TPlayerController.SharedInstance.IsInActionRange(newPointerCell))
                {
                    m_TransformComponent.position = TTilemapManager.SharedInstance.CellToWorld(newPointerCell) + m_Offset;
                    if (!m_SpriteRendererComponent.gameObject.activeInHierarchy) m_SpriteRendererComponent.gameObject.SetActive(true);
                }
                else
                {
                    if (m_SpriteRendererComponent.gameObject.activeInHierarchy) m_SpriteRendererComponent.gameObject.SetActive(false);
                }
            }

            yield return null;
        }
    }
}
