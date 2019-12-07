using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItemsDatabase : MonoBehaviour
{
    [SerializeField] private TItemCollection m_inputCollection;
    public TItemCollection Collection { get; private set; }

    

    private void Awake()
    {
        Collection = Instantiate(m_inputCollection) as TItemCollection;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log(Collection.AllItemsInCollection.Count);
        }
    }


}
