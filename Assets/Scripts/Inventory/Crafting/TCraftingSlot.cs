using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TCraftingSlot : MonoBehaviour
{
    [SerializeField] private Image m_ItemImage;

    private TPlayerController m_myPlayer;

    private TRecipe m_recipeInSlot;

    
    private void Start()
    {
        if (m_recipeInSlot.itemToObtain != null)
        {
            m_ItemImage.sprite = m_recipeInSlot.itemToObtain.Item.ItemSprite;
        }
    }

    #region Manage Recipe in slot

    /// <summary>
    /// Fill this slot with a recipe.
    /// </summary>
    public void FillSlot(TRecipe inCraftableItem)
    {
        gameObject.SetActive(true);
        m_ItemImage.sprite = inCraftableItem.itemToObtain.Item.ItemSprite;
        m_recipeInSlot = inCraftableItem;
    }

    /// <summary>
    /// Craft this recipe.
    /// </summary>
    public void CraftItem()
    {
        m_myPlayer.PlayerCraftComponent.CraftRecipe(m_recipeInSlot);
    }


    public void CancelItem()
    {
        gameObject.SetActive(false);
    }

    #endregion

    /// <summary>
    /// Take the reference to a player.
    /// </summary>
    public void FillPlayerController(TPlayerController inPlayer)
    {
        m_myPlayer = inPlayer;
    }
}
