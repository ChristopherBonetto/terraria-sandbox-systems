using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player's model
/// </summary>
[CreateAssetMenu(fileName = "Data_PlayerName", menuName = "Terraria/Data/Player")]
public class TPlayerData : ScriptableObject
{
    [Header("Defense variables")]
    [SerializeField]
    private int m_MaxHealth;
    public int MaxHealth { get { return m_MaxHealth; } }

    [SerializeField]
    private float m_Defense;
    public float Defense { get { return m_Defense; } }

    [Header("Fight variables")]
    [SerializeField]
    private int m_Damage;
    public int Damage { get { return m_Damage; } }

    [SerializeField]
    private float m_KbResist;
    public float KbResist { get { return m_KbResist; } }

    [Header("Movement variables")]
    [SerializeField]
    private float m_Speed;
    public float Speed { get { return m_Speed; } }

    [SerializeField]
    private float m_JumpForce;
    public float JumpForce { get { return m_JumpForce; } }
}
