using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/GenericItem")]
public class TItem : ScriptableObject
{
    public new string ItemName = "";
        
    [Space]
    public Sprite ItemSprite;
    
}


