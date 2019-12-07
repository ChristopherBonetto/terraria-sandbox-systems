using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IShowDescription : IPointerEnterHandler, IPointerExitHandler
{
    void OnPointerEnter(PointerEventData eventData);
    
    void OnPointerExit(PointerEventData eventData);
}
