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

    public bool InventoryIsOpen = false;

    private void OnEnable()
    {
        TItemHandler.OnDragEvent += ChangeImageItemInHand;
        TItemHandler.OnDragEvent += StartHoldingItem;
        //TItemHandler.OnDragEvent += RestoreColorToSelectedSlotWhenDragged;

        TItemHandler.OnStopDragEvent += DisableImageItemInHand;

        TItemHandler.OnDropEvent += DisableImageItemInHand;

    }
    private void OnDisable()
    {
        TItemHandler.OnDragEvent -= ChangeImageItemInHand;
        TItemHandler.OnDragEvent -= StartHoldingItem;
        //TItemHandler.OnDragEvent -= RestoreColorToSelectedSlotWhenDragged;

        TItemHandler.OnStopDragEvent -= DisableImageItemInHand;

        TItemHandler.OnDropEvent -= DisableImageItemInHand;
    }


    private void Awake()
    {
        Instance = this;
    }

    public void RestoreColorToSelectedSlotWhenDragged()
    {
        if (TItemHandler.Instance.CurrentSelectedItem != null)
        {
            UIManager.Instance.ChangeColorFromImage(TItemHandler.Instance.CurrentSelectedItem.m_slotImage, Color.white);
        }
    }

    

    void Start()
    {
        m_startingInventorySize = m_inventoryUI.sizeDelta;

        InstantiateSlotsInInventory();
        DisableImageItemInHand();

        OpenCloseInventory(InventoryIsOpen);        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryIsOpen = !InventoryIsOpen;
            OpenCloseInventory(InventoryIsOpen);
        }
    }

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

    public void DisableButtons()
    {
        for(int i = 10; i < m_InventorySlotsUI.Count; i++)
        {
            DisableEnableItemUI(m_InventorySlotsUI[i]);
        }
    }

    public void AddSlotToInventoryUI(GameObject slotToAdd)
    {
        if (!m_InventorySlotsUI.Contains(slotToAdd))
        {
            m_InventorySlotsUI.Add(slotToAdd);
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

    public void ChangeSpriteFromImage(Image imageToChange, Sprite spriteToView)
    {
        imageToChange.sprite = spriteToView;
    }
    public void ChangeColorFromImage(Image imageToChange, Color newColor)
    {
        imageToChange.color = newColor;
    }

    public void OpenCloseInventory(bool isClose)
    {
        if (isClose)
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

    public void ChangeImageItemInHand()
    {
        m_itemInHandUI.gameObject.SetActive(true);
        ChangeSpriteFromImage(m_itemInHandUI, TItemHandler.Instance.ItemDraggedInHand.StatsOfThisItem.Item.ItemSprite);
        //m_itemInHandUI.sprite = TItemHandler.Instance.ItemDraggedInHand.Item.ItemSprite;
    }

    public void ItemFollowMousePosition()
    {
        m_itemInHandUI.transform.position = Input.mousePosition;
    }

    public void DisableImageItemInHand()
    {
        m_itemInHandUI.gameObject.SetActive(false);
        m_itemInHandUI.sprite = null;
    }


    public void StartHoldingItem()
    {
        StartCoroutine(ItemInHand());
    }

    IEnumerator ItemInHand()
    {
        while (TItemHandler.Instance.ItemDraggedInHand != null)
        {
            ItemFollowMousePosition();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
}
