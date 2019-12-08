using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player's model
/// </summary>
[CreateAssetMenu(fileName = "Data_PlayerName", menuName = "Terraria/Data/Player")]
public class TPlayerData : ScriptableObject
{
    /// <summary>
    /// All stats in common between enemy, weapon buff, armor buff...
    /// </summary>
    public TStatistics Statistics;

    [Header("Action variables")]
    public float MaxActionDistance;
}
