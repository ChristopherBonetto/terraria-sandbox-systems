using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List that contains items with same type.
/// </summary>
[CreateAssetMenu(fileName = "ItemList", menuName = "ItemCollection/NewListOfItems")]
public class TItemList : ScriptableObject
{
    public List<TItem> List = new List<TItem>();
}
