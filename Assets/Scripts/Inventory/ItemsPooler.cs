using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PooledItem
{
    public GameObject Item;
    public int Quantity;
}


public class ItemsPooler : MonoBehaviour
{
    
    [SerializeField]
    private List<PooledItem> m_poolItems = new List<PooledItem>();

    private void Start()
    {
        InstantiatePooledItems();
    }

    public void InstantiatePooledItems()
    {
        foreach(PooledItem item in m_poolItems)
        {
            if(item.Item != null)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    GameObject clone = Instantiate(item.Item) as GameObject;
                }
            }
        }
    }
}
