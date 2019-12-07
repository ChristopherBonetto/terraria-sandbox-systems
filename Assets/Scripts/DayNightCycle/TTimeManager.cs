using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TTimeManager : MonoBehaviour
{
    public static TTimeManager SharedInstance { get; private set; }

    public TTimeSettings TimeSettings { get { return m_TimeSettings; } }

    public TDayTime CurrentTime { get { return m_CurrentTime; } }

    [SerializeField] private TTimeSettings m_TimeSettings;

    private bool m_IsNight;

    private TDayTime m_CurrentTime;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        m_CurrentTime = m_TimeSettings.GameStartTime;


        TEventManager.TriggerEvent(TEventID.OnTimeStarted, m_CurrentTime);

        m_IsNight = m_TimeSettings.IsStartingDuringNight;

        StartCoroutine(DayTimeUpdate());
    }

    private IEnumerator DayTimeUpdate()
    {
        while (Application.isPlaying)
        {
            m_CurrentTime.Minutes++;

            TEventManager.TriggerEvent(TEventID.OnMinutePassed, m_CurrentTime);

            if (m_IsNight)
            {
                if (m_CurrentTime == m_TimeSettings.DayStartTime)
                {
                    m_IsNight = false;
                    TEventManager.TriggerEvent(TEventID.OnDayNightChanged, m_IsNight);
                }
            }
            else
            {
                if (m_CurrentTime == m_TimeSettings.NightStartTime)
                {
                    m_IsNight = true;
                    TEventManager.TriggerEvent(TEventID.OnDayNightChanged, m_IsNight);
                }
            }

            yield return new WaitForSecondsRealtime(m_TimeSettings.DayMinuteDuration);
        }
    }
}
