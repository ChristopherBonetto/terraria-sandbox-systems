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



    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
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


}
