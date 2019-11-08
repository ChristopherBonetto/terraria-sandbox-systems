using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject MainCanvas;

    [SerializeField] private GameObject m_inventoryItemsHolder;
    [SerializeField] private GameObject m_slotPrefab;

    [SerializeField] private int m_slotsNumber;
    private int m_slotCounter = 0;

    [SerializeField] private Image m_itemInHandUI;



    private void OnEnable()
    {
        ItemHandler.OnDragEvent += ChangeImageItemInHand;
        ItemHandler.OnStopDragEvent += DisableImageItemInHand;
    }
    private void OnDisable()
    {
        ItemHandler.OnDragEvent -= ChangeImageItemInHand;
        ItemHandler.OnStopDragEvent -= DisableImageItemInHand;
    }


    private void Awake()
    {
        Instance = this;
    }

    

    void Start()
    {
        InstantiateSlotsInInventory();
        
    }

    

    public void InstantiateSlotsInInventory()
    {
        if(m_slotCounter <= m_slotsNumber)
        {
            GameObject slot = Instantiate(m_slotPrefab) as GameObject;
            slot.transform.parent = m_inventoryItemsHolder.transform;
            slot.transform.localScale = new Vector3(1,1,1);
            Inventory.Instance.AddToInventory(slot.GetComponentInChildren<InventorySlot>());
            m_slotCounter++;
            InstantiateSlotsInInventory();
        }
        else
        {
            m_slotCounter = 0;
            return;
        }
    }


    public void ChangeImageItemInHand()
    {
        m_itemInHandUI.gameObject.SetActive(true);
        m_itemInHandUI.sprite = ItemHandler.Instance.itemInHand.ItemSprite;
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
}
