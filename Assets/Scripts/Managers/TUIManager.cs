using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TUIManager : MonoBehaviour
{
    public static TUIManager SharedInstance { get; private set; }

    [SerializeField] private Image m_itemInHandUI;

    #region Inventory

    [SerializeField] private GameObject m_inventoryItemsHolder;
    [SerializeField] private GameObject m_slotPrefab;

    public bool m_isInventoryUIOpen { get; private set; }

    [SerializeField] private RectTransform m_inventoryUI;
    private Vector2 m_startingInventorySize;

    public List<GameObject> m_InventorySlotsUI { get; private set; } = new List<GameObject>();
    
    #endregion

    #region Crafting

    [SerializeField] private GameObject m_craftingBar;
    public List<Button> CraftableSlots { get; private set; } = new List<Button>();

    [SerializeField] private Scrollbar m_craftingScrollBar;

    #endregion


    private void OnEnable()
    {
        TEventManager.SubscribeTo<bool>(TEventID.OnInventoryOpen, OpenCloseInventory);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<bool>(TEventID.OnInventoryOpen, OpenCloseInventory);
    }


    private void Awake()
    {
        SharedInstance = this;

        m_startingInventorySize = m_inventoryUI.sizeDelta;
    }

    private void Start()
    {
        m_itemInHandUI.gameObject.SetActive(false);
    }


    #region OpenClose Inventory
    
    //What happens to the ui when the inventory is open or close
    public void OpenCloseInventory(bool inIsOpen)
    {
        m_isInventoryUIOpen = inIsOpen;

        if (m_isInventoryUIOpen == false)
        {
            m_inventoryUI.sizeDelta = new Vector2(m_inventoryUI.sizeDelta.x, 85);
        }
        else
        {
            m_inventoryUI.sizeDelta = m_startingInventorySize;
            
        }
        DisableEnableItemUI(m_craftingBar);
        DisableButtons();
    }

    public void DisableButtons()
    {
        for (int i = 10; i < m_InventorySlotsUI.Count; i++)
        {
            DisableEnableItemUI(m_InventorySlotsUI[i]);
        }
    }

    public void DisableEnableItemUI(GameObject item)
    {
        if (item.activeInHierarchy)
        {
            item.SetActive(false);
        }
        else
        {
            item.SetActive(true);
        }
    }
    #endregion

    #region List of Buttons (Inventory / Crafting)
    public void AddInventorySlotUI(GameObject inSlotToAdd)
    {
        if (!m_InventorySlotsUI.Contains(inSlotToAdd))
        {
            m_InventorySlotsUI.Add(inSlotToAdd);
        }
    }

    public void AddCraftingButton(GameObject inObj)
    {
        Button tempButton = inObj.GetComponent<Button>();

        if(tempButton != null)
        {
            if (!CraftableSlots.Contains(tempButton))
            {
                CraftableSlots.Add(tempButton);
            }
        }
    }
    #endregion

    public void ScrollCraftingBar(float inScrollSpeed)
    {
        if(m_isInventoryUIOpen)
        m_craftingScrollBar.value += inScrollSpeed;
    }
    
}
