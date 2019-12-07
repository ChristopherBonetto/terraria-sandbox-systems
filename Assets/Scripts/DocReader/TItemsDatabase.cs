using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItemsDatabase : MonoBehaviour
{
    public static TItemsDatabase SharedInstance;

    [SerializeField] private TItemCollection m_inputCollection;
    public TItemCollection Collection { get; private set; }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TItem>(TEventID.OnSearchItem, SearchItemInCollection);
    }
    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TItem>(TEventID.OnSearchItem, SearchItemInCollection);
    }


    private void Awake()
    {
        SharedInstance = this;

        Collection = Instantiate(m_inputCollection) as TItemCollection;
    }


    public void SearchItemInCollection(TItem inItem)
    {
        for(int i = 0; i < Collection.AllItemsInCollection.Count; i++)
        {
            if(Collection.AllItemsInCollection[i].ItemName == inItem.ItemName)
            {
                TEventManager.TriggerEvent<List<string>>(TEventID.OnShowTextDescription, Collection.AllItemsInCollection[i].textValues);
                return;
            }
        }
    }
}
