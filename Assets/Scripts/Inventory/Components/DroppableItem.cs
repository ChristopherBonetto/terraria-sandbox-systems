using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private InventorySlot m_slot;


    private void OnEnable()
    {
        ItemHandler.OnDropEvent += TakeItemInHand;
    }
    private void OnDisable()
    {
        ItemHandler.OnDropEvent -= TakeItemInHand;
    }


    private void Awake()
    {
        m_slot = gameObject.GetComponent<InventorySlot>();
    }




    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit");
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemHandler.Instance.DropItemEvent();
    }




    public void TakeItemInHand()
    {
        if(ItemHandler.Instance.itemInHand != null)
        {
            m_slot.ItemInSlot = ItemHandler.Instance.itemInHand;
            Debug.Log(m_slot.ItemInSlot);
        }
    }
}
