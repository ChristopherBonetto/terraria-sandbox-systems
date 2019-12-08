using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will contains a Collection of items.
/// </summary>
public class TItemsDatabase : MonoBehaviour
{
    public static TItemsDatabase SharedInstance;

    //Scriptable that it contains all items in game.
    [SerializeField] private TItemCollection m_inputCollection;

    //Used to have a copy of all items in game.
    public TItemCollection Collection { get; private set; }

    #region Events

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TItem>(TEventID.OnSearchItem, SearchItemInCollection);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TItem>(TEventID.OnSearchItem, SearchItemInCollection);
    }

    #endregion

    private void Awake()
    {
        SharedInstance = this;

        //Create the copy of the collection.
        Collection = Instantiate(m_inputCollection) as TItemCollection;
    }

    #region Search Item

    /// <summary>
    /// Giving an item in iput, this method check if exist into the collection and trigger the event to show his description.
    /// </summary>

    public void SearchItemInCollection(TItem inItem)
    {
        for(int i = 0; i < Collection.AllItemsInCollection.Count; i++)
        {
            if(Collection.AllItemsInCollection[i].ItemName == inItem.ItemName)
            {
                TEventManager.TriggerEvent<List<string>>(TEventID.OnShowTextDescription, Collection.AllItemsInCollection[i].m_textValues);
                return;
            }
        }
    }

    #endregion
}
