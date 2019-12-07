using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Light settings/Lighting State"))]
public class TLightingState : ScriptableObject
{
    public Color BackgroundColor { get { return m_BackgroundColor; } }
    public Color LightColor { get { return m_LightColor; } }

    [SerializeField] private Color m_BackgroundColor;
    [SerializeField] private Color m_LightColor;
}
