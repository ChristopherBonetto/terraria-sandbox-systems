using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewItem", fileName = "Item")]
public class ItemScriptable : ScriptableObject
{
    public new string ItemName = "";
    
    [Space]
    
    public int ItemQuantity;

    [Space]

    public int Durability = 1;
    public int Attack = 1;
    public int Defence = 1;
    public float TimeToAttack = 1;
    
    [Space]
    public Sprite ItemSprite;
    
}
