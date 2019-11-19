using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private GameObject m_inventoryItemsHolder;
    [SerializeField] private GameObject m_slotPrefab;

    [SerializeField] private RectTransform m_inventoryUI;
    private Vector2 m_startingInventorySize;
    
    [SerializeField] private Image m_itemInHandUI;

    public List<GameObject> m_InventorySlotsUI = new List<GameObject>();

    private bool m_inventoryInUiIsOpen = false;

    private void OnEnable()
    {
        TItemHandler.OnSelectEvent += DragSlotInHandUI;

        TItemHandler.OnDeselectEvent += DropSlotInHandUI;
    }
    private void OnDisable()
    {
        TItemHandler.OnSelectEvent -= DragSlotInHandUI;

        TItemHandler.OnDeselectEvent -= DropSlotInHandUI;
    }


    private void Awake()
    {
        Instance = this;

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
    
    public void AddSlotToInventoryUI(GameObject slotToAdd)
    {
        if (!m_InventorySlotsUI.Contains(slotToAdd))
        {
            m_InventorySlotsUI.Add(slotToAdd);
        }
    }
    #endregion


    #region OpenClose Inventory
    
    //What happens to the ui when the inventory is open or close
    public void OpenCloseInventory(bool isOpen)
    {
        m_inventoryInUiIsOpen = isOpen;

        if (!isOpen)
        {
            m_inventoryUI.sizeDelta = new Vector2(m_inventoryUI.sizeDelta.x, 85);
        }
        else
        {
            m_inventoryUI.sizeDelta = m_startingInventorySize;
        }
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
           

    #region Refresh ItemInHandPosition
    public void StartHoldingItem()
    {
        StartCoroutine(ItemInHand());
    }

    public void ItemFollowMousePosition()
    {
        m_itemInHandUI.transform.position = Input.mousePosition;
    }

    IEnumerator ItemInHand()
    {
        while (m_itemInHandUI.gameObject.active)
        {
            ItemFollowMousePosition();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
    #endregion

   
    #region Drag and Drop event for UI
    //Method used with event to start drag a slot
    public void DragSlotInHandUI(TInventorySlot slot)
    {
        if(slot.ItemInSlot.Item != null)
        {
            if (m_inventoryInUiIsOpen)
            {
                m_itemInHandUI.gameObject.SetActive(true);
                m_itemInHandUI.sprite = slot.m_slotImage.sprite;
                
                slot.m_slotImage.sprite = null;

                StartHoldingItem();
            }
        }
    }

    //Method used with event to drop a slot
    public void DropSlotInHandUI(TInventorySlot slot)
    {
        if (m_inventoryInUiIsOpen)
        {            
            slot.m_slotImage.sprite = m_itemInHandUI.sprite;
            m_itemInHandUI.gameObject.SetActive(false);
        }
    }
    #endregion
}
