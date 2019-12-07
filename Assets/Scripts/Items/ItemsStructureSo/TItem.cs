using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Item/Generic Item")]
public class TItem : ScriptableObject
{
    public bool DepleteOnUse { get { return m_DepleteOnUse; } }

    [Space]
    public new string ItemName = "";

    [Space]
    public Sprite ItemSprite;

    [SerializeField] private bool m_DepleteOnUse;
    
}


