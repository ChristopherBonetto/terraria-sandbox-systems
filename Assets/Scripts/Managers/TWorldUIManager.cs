using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TWorldUIManager : MonoBehaviour
{
    [Header("Collection Text")]
    [SerializeField] private Vector3 m_CollectionTextOffset;
    [SerializeField] private float m_CollectionTextDuration;

    private Dictionary<TItem, TCollectionText> m_CollectionTexts;

    private void Awake()
    {
        m_CollectionTexts = new Dictionary<TItem, TCollectionText>();
    }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TItemQuantity, Vector3>(TEventID.OnItemCollected, ShowCollectionText);
        TEventManager.SubscribeTo<TItem>(TEventID.OnCollectionTextDisabled, RemoveCollectionText);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TItemQuantity, Vector3>(TEventID.OnItemCollected, ShowCollectionText);
        TEventManager.UnsubscribeFrom<TItem>(TEventID.OnCollectionTextDisabled, RemoveCollectionText);
    }


    private void ShowCollectionText(TItemQuantity inItemQuantity, Vector3 inWorldPosition)
    {
        TCollectionText collectionText;

        if (m_CollectionTexts.ContainsKey(inItemQuantity.Item))
        {
            collectionText = m_CollectionTexts[inItemQuantity.Item];

            collectionText.IncrementAmount(inItemQuantity.Amount);
        }
        else
        {
            collectionText = ObjectPooler.SharedInstance.GetPooledObject("FloatingText").GetComponent<TCollectionText>();
            
            collectionText.Show(inItemQuantity, m_CollectionTextDuration);

            m_CollectionTexts.Add(inItemQuantity.Item, collectionText);
        }

        collectionText.RectTransformComponent.position = inWorldPosition + m_CollectionTextOffset;
    }

    private void RemoveCollectionText(TItem inItem)
    {
        m_CollectionTexts.Remove(inItem);
    }
}
