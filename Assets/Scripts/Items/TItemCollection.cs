using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCollection", menuName = "ItemCollection/NewCollection")]
public class TItemCollection : ScriptableObject
{
    [SerializeField] private List<TItemList> Collection = new List<TItemList>();

    public List<TItem> AllItemsInCollection { get; private set; } = new List<TItem>();

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
