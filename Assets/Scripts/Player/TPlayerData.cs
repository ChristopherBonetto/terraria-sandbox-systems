using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player's model
/// </summary>
[CreateAssetMenu(fileName = "Data_PlayerName", menuName = "Terraria/Data/Player")]
public class TPlayerData : ScriptableObject
{
    //[Header("Defense variables")]
    //public int MaxHealth;
    //public float Defense; 


    //[Header("Fight variables")]
    //public int Damage;
    //public float AttackSpeed;
    //[Range(0, 100)] public float KbResist;

    //[Header("Movement variables")]
    //public float Speed;
    //public float JumpForce;


    public TStatistics Statistics;

    [Header("Action variables")]
    public float MaxActionDistance;
}
