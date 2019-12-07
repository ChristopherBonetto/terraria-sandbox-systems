using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Light settings/Light Phase"))]
public class TLightPhase : ScriptableObject
{
    public TLightingState State { get { return m_State; } }
    public TLightingState BlendState { get { return m_BlendState; } }

    public TDayTime StartTime { get { return m_StartTime; } }
    public TDayTime BlendStartTime { get { return m_StartTime - m_BlendDuration / 2; } }
    public TDayTime BlendDuration { get { return m_BlendDuration; } }

    [SerializeField] private TLightingState m_State;
    [SerializeField] private TDayTime m_StartTime;

    [SerializeField] private TLightingState m_BlendState;
    [SerializeField] private TDayTime m_BlendDuration;
}
