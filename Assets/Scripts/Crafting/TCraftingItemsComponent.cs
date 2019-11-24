using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCraftingItemsComponent : MonoBehaviour
{
    public List<TItem> CraftableItem;

    private TPlayerController m_myPlayer;




    private void Awake()
    {
        m_myPlayer = GetComponent<TPlayerController>();
    }

    private void Update()
    {
        if (m_myPlayer.PlayerInventory.InventoryIsOpen)
        {
            FindAvaibleItems();
        }


        if (Input.GetKeyDown(KeyCode.O))
        {
            FindCraftableItems();
        }
    }

    public void FindAvaibleItems()
    {
        CraftableItem = new List<TItem>();
        CraftableItem = m_myPlayer.PlayerInventory.ItemsInInventory();
    }

    public void FindCraftableItems()
    {
        TRecipeContainer.SharedIstance.CheckCraftableItem(CraftableItem[0]);
    }

}
