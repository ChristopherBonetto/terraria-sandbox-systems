using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCraftingItemsComponent : MonoBehaviour
{
    public List<TItemQuantity> CraftableItem;

    private TPlayerController m_myPlayer;




    private void Awake()
    {
        m_myPlayer = GetComponent<TPlayerController>();
    }

    private void Update()
    {
        if (m_myPlayer.PlayerInventory.InventoryIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                FindAvaibleItems();
            }
            
        }


        
    }

    public void FindAvaibleItems()
    {
        CraftableItem = new List<TItemQuantity>();
        CraftableItem = m_myPlayer.PlayerInventory.ItemsInInventory();
        TRecipeContainer.SharedIstance.CheckCraftableItem(CraftableItem);
    }
    
}
