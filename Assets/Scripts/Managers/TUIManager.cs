using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TUIManager : MonoBehaviour
{
    public static TUIManager SharedInstance { get; private set; }

    [Header("Equipment")]
    [SerializeField] private GameObject m_equipmentPanel;

    #region Description

    [Space, SerializeField] private GameObject m_descriptionPanel;
    private Text m_descriptionText;

    private List<string> m_listOfTexts = new List<string>();

    #endregion

    #region Inventory

    [Header("Inventory")]
    [SerializeField] private GameObject m_inventoryItemsHolder;
    [SerializeField] private RectTransform m_inventoryUI;

    private Vector2 m_startingInventorySize;

    public List<GameObject> m_InventorySlotsUI { get; private set; } = new List<GameObject>();

    public bool m_isInventoryUIOpen { get; private set; }

    #endregion

    #region Crafting

    public List<Button> CraftableSlots { get; private set; } = new List<Button>();

    [Header("Crafting")]
    [SerializeField] private GameObject m_craftingBar;
    [SerializeField] private Scrollbar m_craftingScrollBar;

    #endregion


    private void OnEnable()
    {
        TEventManager.SubscribeTo<bool>(TEventID.OnInventoryOpen, OpenCloseInventory);
        TEventManager.SubscribeTo<bool>(TEventID.OnOpenCloseDescription, OpenCloseDescription);

        TEventManager.SubscribeTo<List<string>>(TEventID.OnShowTextDescription, ShowDescriptionText);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<bool>(TEventID.OnInventoryOpen, OpenCloseInventory);
        TEventManager.UnsubscribeFrom<bool>(TEventID.OnOpenCloseDescription, OpenCloseDescription);

        TEventManager.UnsubscribeFrom<List<string>>(TEventID.OnShowTextDescription, ShowDescriptionText);
    }


    private void Awake()
    {
        SharedInstance = this;

        m_startingInventorySize = m_inventoryUI.sizeDelta;

        m_descriptionText = m_descriptionPanel.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        OpenCloseDescription(false);
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
        DisableEnableItemUI(m_equipmentPanel);
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
    
    public void OpenCloseDescription(bool inIsOpen)
    {
        m_descriptionPanel.SetActive(inIsOpen);
    }

    public void ShowDescriptionText(List<string> inList)
    {
        
        if(inList != null)
        {
            for (int i = 0; i < inList.Count; i++)
            {
                //if(i == 0)
                //{
                //    m_descriptionText.text.
                //}
                m_descriptionText.text = m_descriptionText.text + inList[i].ToString() + "\n";
            }
        }
        else
        {
            m_descriptionText.text = null;
        }
        
    }
}
