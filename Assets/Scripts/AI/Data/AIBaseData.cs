using UnityEngine;

namespace Terraria.AI
{
    public enum EnemyType
    {
        None,
        Slime,
        Undead,
        Goblin,
    }

    [CreateAssetMenu(fileName = "Data_MonsterName", menuName = "Terraria/AI/Data")]
    public class AIBaseData : ScriptableObject
    {
        [Header("Identification")]

        [SerializeField] private string m_MonsterName;
        [SerializeField] private EnemyType m_Type;


        [Header("Stats")]
        public TStatistics Statistics;


        public string MonsterName => m_MonsterName;
        public EnemyType Type => m_Type;
    }
}
