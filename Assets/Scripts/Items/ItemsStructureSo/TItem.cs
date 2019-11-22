using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TItem : ScriptableObject
{

    public bool DepleteOnUse { get { return m_DepleteOnUse; } }

    public new string ItemName = "";
        
    [Space]
    public Sprite ItemSprite;

    [SerializeField] private bool m_DepleteOnUse;

    public abstract bool Use(TPlayerController inUser, TPointerData inData);
    
}


