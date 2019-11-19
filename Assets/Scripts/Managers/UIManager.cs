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

    public bool InventoryIsOpen = false;

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
    }
        
    void Start()
    {
        m_startingInventorySize = m_inventoryUI.sizeDelta;

        DisableImageItemInHand();
        
        OpenCloseInventory(InventoryIsOpen);        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeOpenCloseInventoryBool();
        }
    }

    #region Starting Instantiate Buttons
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
    public void ChangeOpenCloseInventoryBool()
    {
        InventoryIsOpen = !InventoryIsOpen;
        TItemHandler.Instance.CurrentSelectedItem = null;
        OpenCloseInventory(InventoryIsOpen);
    }

    public void OpenCloseInventory(bool isOpen)
    {
        if (!isOpen)
        {
            m_inventoryUI.sizeDelta = new Vector2(m_inventoryUI.sizeDelta.x, 85);
            DisableButtons();
        }
        else
        {
            m_inventoryUI.sizeDelta = m_startingInventorySize;
            DisableButtons();
        }
    }

    public void DisableButtons()
    {
        for (int i = 10; i < m_InventorySlotsUI.Count; i++)
        {
            DisableEnableItemUI(m_InventorySlotsUI[i]);
        }
    }
    #endregion



    public void DisableImageItemInHand()
    {
        m_itemInHandUI.gameObject.SetActive(false);
        m_itemInHandUI.sprite = null;
    }

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

    public void DragSlotInHandUI(TInventorySlot slot)
    {
        if(slot.ItemInSlot.Item != null)
        {
            if (InventoryIsOpen)
            {
                m_itemInHandUI.gameObject.SetActive(true);
                m_itemInHandUI.sprite = slot.m_slotImage.sprite;
                
                slot.m_slotImage.sprite = null;

                StartHoldingItem();
            }
        }
    }

    public void DropSlotInHandUI(TInventorySlot slot)
    {
        if (InventoryIsOpen)
        {            
            slot.m_slotImage.sprite = m_itemInHandUI.sprite;
            m_itemInHandUI.gameObject.SetActive(false);
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

}
