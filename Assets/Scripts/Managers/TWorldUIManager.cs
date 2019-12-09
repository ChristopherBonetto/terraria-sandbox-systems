using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TWorldUIManager : MonoBehaviour
{
    #region Serialized varibables

    [Header("Collection Text")]
    [SerializeField] private Vector3 m_CollectionTextOffset;
    [SerializeField] private float m_CollectionTextDuration;

    #endregion

    #region Private variables

    private Dictionary<TItem, TCollectionText> m_CollectionTexts;

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        // Init collection texts dictionary
        m_CollectionTexts = new Dictionary<TItem, TCollectionText>();
    }

    private void OnEnable()
    {
        // Subscribe to events
        TEventManager.SubscribeTo<TItemQuantity, Vector3>(TEventID.OnItemCollected, ShowCollectionText);
        TEventManager.SubscribeTo<TItem>(TEventID.OnCollectionTextDisabled, RemoveCollectionText);
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        TEventManager.UnsubscribeFrom<TItemQuantity, Vector3>(TEventID.OnItemCollected, ShowCollectionText);
        TEventManager.UnsubscribeFrom<TItem>(TEventID.OnCollectionTextDisabled, RemoveCollectionText);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Shows a collection text at the specified position.
    /// </summary>
    /// <param name="inItemQuantity">Quantity collected.</param>
    /// <param name="inWorldPosition">Collection position.</param>
    private void ShowCollectionText(TItemQuantity inItemQuantity, Vector3 inWorldPosition)
    {
        TCollectionText collectionText;

        // Case 1: a collection text for the Item is already visible
        if (m_CollectionTexts.ContainsKey(inItemQuantity.Item))
        {
            // Increment the amount on the text
            collectionText = m_CollectionTexts[inItemQuantity.Item];
            collectionText.IncrementAmount(inItemQuantity.Amount);
        }

        // Case 2: there's no collection text for the Item yet
        else
        {
            // Get a collection text from Pooler
            collectionText = ObjectPooler.SharedInstance.GetPooledObject("FloatingText").GetComponent<TCollectionText>();
            
            // Show it
            collectionText.Show(inItemQuantity, m_CollectionTextDuration);

            // Add it to the dictionary
            m_CollectionTexts.Add(inItemQuantity.Item, collectionText);
        }

        // Move collection text to correct position
        collectionText.RectTransformComponent.position = inWorldPosition + m_CollectionTextOffset;
    }

    /// <summary>
    /// Removes the collection text for the specified Item from the dictionary.
    /// </summary>
    /// <param name="inItem"></param>
    private void RemoveCollectionText(TItem inItem)
    {
        m_CollectionTexts.Remove(inItem);
    }

    #endregion
}
