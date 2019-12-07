using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TTimeManager : MonoBehaviour
{
    [SerializeField] private TTimeSettings m_TimeSettings;

    private bool m_IsNight;

    private int m_CurrentMinutes;
    private int m_DayStartMinutes;
    private int m_NightStartMinutes;

    private void Start()
    {
        m_CurrentMinutes = m_TimeSettings.GameStartTime.ToMinutes();

        m_IsNight = m_TimeSettings.IsStartingDuringNight;

        StartCoroutine(DayTimeUpdate());
    }

    private IEnumerator DayTimeUpdate()
    {
        while (Application.isPlaying)
        {
            if (m_CurrentMinutes < TDayTime.DAY_MINUTES)
                m_CurrentMinutes++;
            else
                m_CurrentMinutes = 0;


            TEventManager.TriggerEvent(TEventID.OnMinutePassed, m_CurrentMinutes);

            if (m_IsNight)
            {
                if (m_CurrentMinutes == m_DayStartMinutes)
                {
                    m_IsNight = false;
                    TEventManager.TriggerEvent(TEventID.OnDayNightChanged, m_IsNight);
                }
            }
            else
            {
                if (m_CurrentMinutes == m_NightStartMinutes)
                {
                    m_IsNight = true;
                    TEventManager.TriggerEvent(TEventID.OnDayNightChanged, m_IsNight);
                }
            }

            yield return new WaitForSeconds(m_TimeSettings.DayMinuteDuration);
        }
    }
}
