using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TWorldUIManager : MonoBehaviour
{
    [Header("Floating Texts")]
    [SerializeField] private Vector3 m_ItemCollectionTextOffset;

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TItemQuantity, Vector3>(TEventID.OnItemCollected, ShowCollectedItemText);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TItemQuantity, Vector3>(TEventID.OnItemCollected, ShowCollectedItemText);
    }


    private void ShowCollectedItemText(TItemQuantity inItemQuantity, Vector3 inWorldPosition)
    {
        Text itemText = ObjectPooler.SharedInstance.GetPooledObject("FloatingText").GetComponent<Text>();

        itemText.transform.position = inWorldPosition + m_ItemCollectionTextOffset;
        itemText.text = inItemQuantity.ToString();

        itemText.gameObject.SetActive(true);
    }
}
