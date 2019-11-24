using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TCraftingSlot : MonoBehaviour
{
    private TPlayerController m_myPlayer;
    private Image m_myImage;

    public TRecipeInfo m_itemInSlot;


    private void Awake()
    {
        m_myImage = gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        if (m_itemInSlot.itemToObtain != null)
        {
            m_myImage.sprite = m_itemInSlot.itemToObtain.Item.ItemSprite;
        }
    }

    public void FillSlot(TRecipeInfo inCraftableItem)
    {
        gameObject.SetActive(true);
        m_myImage.sprite = inCraftableItem.itemToObtain.Item.ItemSprite;
        m_itemInSlot = inCraftableItem;
    }

    public void CraftItem()
    {
        m_myPlayer.PlayerCraftComponent.CraftRecipe(m_itemInSlot);
    }

    public void CancelItem()
    {
        m_itemInSlot = TRecipeInfo.Empty;
        gameObject.SetActive(false);
    }

    public void FillPlayerController(TPlayerController inPlayer)
    {
        m_myPlayer = inPlayer;
    }
}
