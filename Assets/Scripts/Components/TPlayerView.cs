using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TPlayerView : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private Slider m_HealthBar;
    public Slider HealthBar{ get { return m_HealthBar; } }

    private Vector3 m_Scale;

    private void Awake()
    {
        m_Scale = transform.localScale;
    }

    /// <summary>
    /// Update health bar when some damage is taken.
    /// </summary>
    public void UpdateHealthBar(float inValue)
    {
        m_HealthBar.value -= inValue;
    }

    public void Flip(Vector2 inDirection)
    {
        transform.localScale = (Vector3)new Vector2(inDirection.x * m_Scale.x, m_Scale.y);
    }
}
