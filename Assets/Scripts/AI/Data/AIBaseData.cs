using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_MonsterName", menuName = "AI/Data")]
public class AIBaseData : ScriptableObject
{
    [Header("Identification")]

    [SerializeField] private string m_MonsterName;
    [SerializeField] private int m_ID;
    //[SerializeField] private EnemyType Type;

    [Header("Stats")]

    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_Defense;
    [SerializeField] private float m_Damage;
    [SerializeField] private float m_KbResist;

    //[Header("Drops")]

    //[SerializeField] private List<Item> m_Drops;


    public string MonsterName => m_MonsterName;
    public int ID => m_ID;

    public float MaxHealth => m_MaxHealth;
    public float Defense => m_Defense;
    public float Damage => m_Damage;
    public float KbResist => m_KbResist;
}
