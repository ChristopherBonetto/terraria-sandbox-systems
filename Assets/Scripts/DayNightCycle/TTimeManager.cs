using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TTimeManager : MonoBehaviour
{
    public static TTimeManager SharedInstance { get; private set; }

    #region Public properties

    /// <summary>
    /// Time Settings being used.
    /// </summary>
    public TTimeSettings TimeSettings { get { return m_TimeSettings; } }

    /// <summary>
    /// Current time in HH:MM format.
    /// </summary>
    public TDayTime CurrentTime { get { return m_CurrentTime; } }

    /// <summary>
    /// Is currently Night?
    /// </summary>
    public bool IsNight
    {
        get { return m_IsNight; }
        private set
        {
            m_IsNight = value;

            TEventManager.TriggerEvent(TEventID.OnDayNightChanged, m_IsNight);
        }
    }

    #endregion

    #region Serialized variables

    [SerializeField] private TTimeSettings m_TimeSettings;

    #endregion

    #region Private variables

    private bool m_IsNight;

    private TDayTime m_CurrentTime;

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        // Set singleton instance
        SharedInstance = this;
    }

    private void Start()
    {
        // Init current time
        m_CurrentTime = m_TimeSettings.GameStartTime;
        TEventManager.TriggerEvent(TEventID.OnTimeStarted, m_CurrentTime);

        // Init night state
        IsNight = m_TimeSettings.IsStartingDuringNight;

        // Start time update coroutine
        StartCoroutine(DayTimeUpdate());
    }

    #endregion

    #region Private coroutine

    /// <summary>
    /// Coroutine updating time each in-game minute.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DayTimeUpdate()
    {
        while (Application.isPlaying)
        {
            // Increase minutes
            m_CurrentTime.Minutes++;
            TEventManager.TriggerEvent(TEventID.OnMinutePassed, m_CurrentTime);

            // Update night state
            if (IsNight)
            {
                if (m_CurrentTime == m_TimeSettings.DayStartTime) IsNight = false;
            }
            else
            {
                if (m_CurrentTime == m_TimeSettings.NightStartTime) IsNight = true;
            }

            // Wait one in-game minute
            yield return new WaitForSecondsRealtime(m_TimeSettings.DayMinuteDuration);
        }
    }

    #endregion
}
