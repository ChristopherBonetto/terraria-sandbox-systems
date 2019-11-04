using UnityEngine;
using System.Collections;

/// <summary>
/// Base AI,
/// doesn't move or react.
/// </summary>
[RequireComponent(typeof(DefenseComponent))]
public class BaseAI : MonoBehaviour
{
    [SerializeField] private AIBaseData m_Data;
    public AIBaseData Data => m_Data;

    // Component
    private IDefend m_DefenseComponent;
    public IDefend DefenseComponent
    {
        get
        {
            if (m_DefenseComponent == null)
                m_DefenseComponent = GetComponent<IDefend>();
            return m_DefenseComponent;
        }
    }

    protected virtual void OnEnable()
    {
        // KB resist
        // Subscribe.
    }

    protected virtual void Start()
    {
        DefenseComponent.Init(/*inMaxHealth:*/ Data.MaxHealth, /*inDefense:*/ Data.Defense);
    }

    protected virtual void OnDisable()
    {
        // KB resist
        // Unsuscribe.
    }
}
