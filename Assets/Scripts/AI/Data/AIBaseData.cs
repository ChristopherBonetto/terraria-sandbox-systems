using UnityEngine;

namespace Terrria.AI
{
    public enum EnemyType
    {
        None,
        Slime,
        Undead,
    }

    [CreateAssetMenu(fileName = "Data_MonsterName", menuName = "Terraria/AI/Data")]
    public class AIBaseData : ScriptableObject
    {
        [Header("Identification")]

        [SerializeField] private string m_MonsterName;
        [SerializeField] private int m_ID;
        [SerializeField] private EnemyType m_Type;

        [Header("Stats")]

        [SerializeField] private float m_MaxHealth;
        [SerializeField] private float m_Defense;
        [SerializeField] private float m_Damage;
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_JumpForce;
        [SerializeField] private float m_KbResist;

        //[Header("Drops")]

        //[SerializeField] private List<Item> m_Drops;


        // Getters
        public string MonsterName => m_MonsterName;
        public int ID => m_ID;
        public EnemyType Type => m_Type;

        public float MaxHealth => m_MaxHealth;
        public float Defense => m_Defense;
        public float Damage => m_Damage;
        public float Speed => m_Speed;
        public float JumpForce => m_JumpForce;
        public float KbResist => m_KbResist;
    }
}
