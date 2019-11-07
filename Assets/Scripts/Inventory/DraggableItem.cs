using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform m_parentToThisItem = null;

    private CanvasGroup m_canvasGroup;


    private void Awake()
    {
        m_canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        m_parentToThisItem = this.transform.parent;
        
        SetNewParent(UIManager.Instance.MainCanvas.transform);

        m_canvasGroup.blocksRaycasts = false;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        SetNewParent(m_parentToThisItem);
        

        m_canvasGroup.blocksRaycasts = true;
    }


    public void SetNewParent(Transform inNewParent)
    {
        this.transform.parent = null;
        this.transform.SetParent(inNewParent);
        this.transform.localPosition = new Vector3(0, 0, 0);
    }
}
