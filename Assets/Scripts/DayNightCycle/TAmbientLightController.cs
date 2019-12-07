using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

[RequireComponent(typeof(Light2D))]
public class TAmbientLightController : MonoBehaviour
{
    [SerializeField] private TLightPhase[] m_Phases;

    private Camera m_MainCamera;
    private Light2D m_LightSource;

    private float m_Percentage;

    private int m_NextPhaseIndex;

    private void Awake()
    {
        m_MainCamera = Camera.main;
        m_LightSource = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TDayTime>(TEventID.OnMinutePassed, StartBlend);
        TEventManager.SubscribeTo<TDayTime>(TEventID.OnTimeStarted, InitPhase);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TDayTime>(TEventID.OnMinutePassed, StartBlend);
        TEventManager.UnsubscribeFrom<TDayTime>(TEventID.OnTimeStarted, InitPhase);
    }

    private void InitPhase(TDayTime inStartTime)
    {
        int count = m_Phases.Length;

        int currentMinutesFromBlendStart;
        int blendDurationMinutes;
        int nextPhaseMinutesFromBlendStart;

        int followingPhase;

        for (int i = 0; i < count; i++)
        {
            followingPhase = RepeatIndex(i + 1, count);

            currentMinutesFromBlendStart = TDayTime.MinutesBetween(m_Phases[i].BlendStartTime, inStartTime);
            blendDurationMinutes = m_Phases[i].BlendDuration.ToMinutes();
            nextPhaseMinutesFromBlendStart = TDayTime.MinutesBetween(m_Phases[i].BlendStartTime, m_Phases[followingPhase].BlendStartTime);

            if (currentMinutesFromBlendStart < blendDurationMinutes)
            {
                m_NextPhaseIndex = i;
                StartCoroutine(Blend(RepeatIndex(i - 1, count), i, currentMinutesFromBlendStart));
            }
            else if (currentMinutesFromBlendStart < nextPhaseMinutesFromBlendStart)
            {
                m_NextPhaseIndex = followingPhase;
                m_MainCamera.backgroundColor = m_Phases[i].State.BackgroundColor;
                m_LightSource.color = m_Phases[i].State.LightColor;
            }
        }


    }

    private void StartBlend(TDayTime inCurrentTime)
    {
        if (inCurrentTime == m_Phases[m_NextPhaseIndex].BlendStartTime)
            StartCoroutine(Blend(RepeatIndex(m_NextPhaseIndex-1, m_Phases.Length), m_NextPhaseIndex));
    }

    private IEnumerator Blend(int fromIndex, int toIndex, int inStartMinute = 0)
    {
        float halfDuration = m_Phases[toIndex].BlendDuration.ToMinutes() / 2.0f;
        
        float speed = 0.5f / (halfDuration * TTimeManager.SharedInstance.TimeSettings.DayMinuteDuration);

        float secondHalfStart;

        if (inStartMinute < halfDuration)
        {
            secondHalfStart = 0;

            for (float t = inStartMinute / halfDuration; t <= 1; t += speed * Time.deltaTime)
            {
                m_MainCamera.backgroundColor = Color.Lerp(m_Phases[fromIndex].State.BackgroundColor, m_Phases[toIndex].BlendState.BackgroundColor, t);
                m_LightSource.color = Color.Lerp(m_Phases[fromIndex].State.LightColor, m_Phases[toIndex].BlendState.LightColor, t);

                //yield return new WaitForSecondsRealtime(TTimeManager.SharedInstance.TimeSettings.DayMinuteDuration);
                yield return null;
            }
        }
        else
        {
            secondHalfStart = inStartMinute / halfDuration - 1;
        }

        for (float t = secondHalfStart; t <= 1; t += speed * Time.deltaTime)
        {
            m_MainCamera.backgroundColor = Color.Lerp(m_Phases[toIndex].BlendState.BackgroundColor, m_Phases[toIndex].State.BackgroundColor, t);
            m_LightSource.color = Color.Lerp(m_Phases[toIndex].BlendState.LightColor, m_Phases[toIndex].State.LightColor, t);

            //yield return new WaitForSecondsRealtime(TTimeManager.SharedInstance.TimeSettings.DayMinuteDuration);
            yield return null;
        }

        if (m_NextPhaseIndex < m_Phases.Length - 1)
            m_NextPhaseIndex++;
        else
            m_NextPhaseIndex = 0;

    }

    private int RepeatIndex(int index, int length)
    {
        while (index < 0) index += length;
        while (index >= length) index -= length;

        return index;
    }
}