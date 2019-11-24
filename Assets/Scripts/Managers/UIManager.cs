using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager SharedInstance { get; private set; }

    public bool IsInventoryOpen { get; private set; }

    [SerializeField] private GameObject m_craftingBar;

    [SerializeField] private GameObject m_inventoryItemsHolder;
    [SerializeField] private GameObject m_slotPrefab;

    [SerializeField] private RectTransform m_inventoryUI;
    private Vector2 m_startingInventorySize;
    
    [SerializeField] private Image m_itemInHandUI;

    public List<GameObject> m_InventorySlotsUI = new List<GameObject>();

    public List<Button> CraftableSlots = new List<Button>();

    private void Awake()
    {
        SharedInstance = this;

        m_startingInventorySize = m_inventoryUI.sizeDelta;
    }
        
    void Start()
    {
        m_itemInHandUI.gameObject.SetActive(false);

        
    }

    
    #region Starting Instantiate Buttons
    //Instantiate button and return his Tinventory slot
    public TInventorySlot InstantiateSlotInInventory()
    {
        GameObject slot = Instantiate(m_slotPrefab) as GameObject;

        slot.transform.SetParent(m_inventoryItemsHolder.transform);
        slot.transform.localScale = new Vector3(1, 1, 1);

        AddSlotToInventoryUI(slot);

        TInventorySlot tempSlotRef = slot.GetComponentInChildren<TInventorySlot>();

        return tempSlotRef;
    }
    
    public void AddSlotToInventoryUI(GameObject inSlotToAdd)
    {
        if (!m_InventorySlotsUI.Contains(inSlotToAdd))
        {
            m_InventorySlotsUI.Add(inSlotToAdd);
        }
    }
    #endregion


    #region OpenClose Inventory
    
    //What happens to the ui when the inventory is open or close
    public void OpenCloseInventory(bool inIsOpen)
    {
        IsInventoryOpen = inIsOpen;

        if (!inIsOpen)
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
    
}
