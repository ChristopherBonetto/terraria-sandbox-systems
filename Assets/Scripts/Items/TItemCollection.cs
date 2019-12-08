using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable used to contains all the list of the items.
/// </summary>
[CreateAssetMenu(fileName = "ItemCollection", menuName = "ItemCollection/NewCollection")]
public class TItemCollection : ScriptableObject
{
    //Lists of items.
    [SerializeField] private List<TItemList> Collection = new List<TItemList>();

    //Filled to caontains all the items.
    public List<TItem> AllItemsInCollection { get; private set; } = new List<TItem>();


    /// <summary>
    /// For each list instantiates all his elements and add them to <param AllItemsInCollection>
    /// </summary>
    public void Awake()
    {
        for(int i = 0; i < Collection.Count; i++)
        {
            for(int j = 0; j < Collection[i].List.Count; j++)
            {
                TItem tempItem = Instantiate(Collection[i].List[j]);
                tempItem.name = Collection[i].List[j].name;
                
                AllItemsInCollection.Add(tempItem);
            }
        }
    }
}
