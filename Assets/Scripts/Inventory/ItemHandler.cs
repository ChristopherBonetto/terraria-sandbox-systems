using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHandler : MonoBehaviour
{
    public static ItemHandler Instance;

    public ItemScriptable itemInHand = null;



    public delegate void OnBeginDragDelegate(ItemScriptable tempSlottedItem);
    public static OnBeginDragDelegate OnBeginDragEvent;

    public void StartDragItemEvent(ItemScriptable tempSlottedItem)
    {
        if(tempSlottedItem != null)
        {
            itemInHand = tempSlottedItem;
            OnDragEvent();
        }
    }


    public delegate void OnDragDelegate();
    public static OnDragDelegate OnDragEvent;

    public void DragItemEvent()
    {
        if(OnDragEvent != null)
        {
            OnDragEvent();
        }
    }


    public delegate void OnDropDelegate();
    public static OnDropDelegate OnDropEvent;

    public void DropItemEvent()
    {
        if (OnDropEvent != null)
        {
            OnDropEvent();
        }
    }



    private void OnEnable()
    {
        OnBeginDragEvent = StartDragItemEvent;
        OnDragEvent += StartHoldingItem;
    }
    private void OnDisable()
    {
        OnBeginDragEvent = StartDragItemEvent;
        OnDragEvent -= StartHoldingItem;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }
    

    void Update()
    {
        if(itemInHand != null)
        {
            Debug.Log(itemInHand);
        }
        


        if (Input.GetKeyDown(KeyCode.A))
        {
            itemInHand = null;
        }

    }

    

    public void StartHoldingItem()
    {
        StartCoroutine(ItemInHand());
    }

    IEnumerator ItemInHand()
    {
        while(itemInHand != null)
        {            
            UIManager.Instance.ItemFollowMousePosition();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
}
