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

    [SerializeField] private int m_slotsNumber;
    private int m_slotCounter = 0;

    [SerializeField] private Image m_itemInHandUI;

    public List<GameObject> m_InventorySlotsUI = new List<GameObject>();

    private bool InventoryIsOpen = false;

    private void OnEnable()
    {
        TItemHandler.OnSelectEvent += ShowSlotInHand;

        TItemHandler.OnDeselectEvent += HideSlotInHand;
    }
    private void OnDisable()
    {
        TItemHandler.OnSelectEvent -= ShowSlotInHand;

        TItemHandler.OnDeselectEvent -= HideSlotInHand;
    }


    private void Awake()
    {
        Instance = this;
    }
        
    void Start()
    {
        m_startingInventorySize = m_inventoryUI.sizeDelta;

        DisableImageItemInHand();

        InstantiateSlotsInInventory();
        
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
    public void InstantiateSlotsInInventory()
    {
        if(m_slotCounter <= m_slotsNumber)
        {
            GameObject slot = Instantiate(m_slotPrefab) as GameObject;

            slot.transform.SetParent(m_inventoryItemsHolder.transform);
            slot.transform.localScale = new Vector3(1,1,1);

            AddSlotToInventoryUI(slot);
            TInventory.Instance.AddSlotToInventory(slot.GetComponentInChildren<TInventorySlot>());

            m_slotCounter++;
            InstantiateSlotsInInventory();
        }
        else
        {
            m_slotCounter = 0;
            return;
        }
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
        while (TItemHandler.Instance.CurrentSelectedItem != null)
        {
            ItemFollowMousePosition();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
    #endregion

    public void ShowSlotInHand(TInventorySlot slot)
    {
        if(slot.ItemInSlot != null)
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

    public void HideSlotInHand(TInventorySlot slot)
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
