using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TCraftingSlot : MonoBehaviour
{
    [SerializeField] private Image m_ItemImage;

    private TPlayerController m_myPlayer;

    private TRecipe m_itemInSlot;

    private void Start()
    {
        if (m_itemInSlot.itemToObtain != null)
        {
            m_ItemImage.sprite = m_itemInSlot.itemToObtain.Item.ItemSprite;
        }
    }

    public void FillSlot(TRecipe inCraftableItem)
    {
        gameObject.SetActive(true);
        m_ItemImage.sprite = inCraftableItem.itemToObtain.Item.ItemSprite;
        m_itemInSlot = inCraftableItem;
    }

    public void CraftItem()
    {
        m_myPlayer.PlayerCraftComponent.CraftRecipe(m_itemInSlot);
    }

    public void CancelItem()
    {
        //m_itemInSlot = TRecipeInfo.Empty;
        gameObject.SetActive(false);
    }

    public void FillPlayerController(TPlayerController inPlayer)
    {
        m_myPlayer = inPlayer;
    }
}
