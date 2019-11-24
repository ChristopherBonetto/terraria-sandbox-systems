using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TCraftingSlot : MonoBehaviour
{
    public TPlayerController m_myPlayer;

    public TItemQuantity m_itemInSlot;

    private Image m_myImage;

    private void Awake()
    {
        m_myImage = gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        if(m_itemInSlot != null)
        {
            m_myImage.sprite = m_itemInSlot.Item.ItemSprite;
        }
    }

    public void FillSlot(TItemQuantity inItem)
    {
        if(inItem == null)
        {
            gameObject.SetActive(false);
            m_myImage.sprite = null;
            m_itemInSlot = TItemQuantity.Empty;
        }
        else
        {
            gameObject.SetActive(true);
            m_myImage.sprite = inItem.Item.ItemSprite;
            m_itemInSlot = inItem;
        }
        
    }
}
