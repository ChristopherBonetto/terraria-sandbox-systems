using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemList", menuName = "ItemCollection/NewListOfItems")]
public class TItemList : ScriptableObject
{
    public List<TItem> List = new List<TItem>();
}
