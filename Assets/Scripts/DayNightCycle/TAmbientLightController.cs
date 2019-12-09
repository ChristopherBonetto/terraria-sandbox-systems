using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

[RequireComponent(typeof(Light2D))]
public class TAmbientLightController : MonoBehaviour
{
    #region Serialized variables

    /// <summary>
    /// Ambient light phases to run through.
    /// </summary>
    [SerializeField] private TLightPhase[] m_Phases;

    #endregion

    #region Private variables

    // Cached references
    private Camera m_MainCamera;
    private Light2D m_LightSource;

    // Index pointing to next phase to blend into
    private int m_NextPhaseIndex;

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        // Cache references
        m_MainCamera = Camera.main;
        m_LightSource = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        // Subscribe to events
        TEventManager.SubscribeTo<TDayTime>(TEventID.OnMinutePassed, CheckTimeAndStartBlend);
        TEventManager.SubscribeTo<TDayTime>(TEventID.OnTimeStarted, InitPhase);
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        TEventManager.UnsubscribeFrom<TDayTime>(TEventID.OnMinutePassed, CheckTimeAndStartBlend);
        TEventManager.UnsubscribeFrom<TDayTime>(TEventID.OnTimeStarted, InitPhase);
    }

    #endregion


    #region Private methods

    /// <summary>
    /// Wraps the provided index between 0 (included) and length (excluded).
    /// </summary>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    private int RepeatIndex(int index, int length)
    {
        while (index < 0) index += length;
        while (index >= length) index -= length;

        return index;
    }

    /// <summary>
    /// Initializes the phase on startup.
    /// </summary>
    /// <param name="inStartTime"></param>
    private void InitPhase(TDayTime inStartTime)
    {
        int count = m_Phases.Length;

        int currentMinutes;
        int blendEndMinutes;
        int nextPhaseMinutes;

        int followingPhase;

        // Check all phases: since day-time wraps on itself, whole time range is checked this way
        for (int i = 0; i < count; i++)
        {
            // Get following phase index
            followingPhase = RepeatIndex(i + 1, count);

            // Get lengths in minutes from current phase blend start time:
            // Current time
            currentMinutes = TDayTime.MinutesBetween(m_Phases[i].BlendStartTime, inStartTime);

            // Blend end time
            blendEndMinutes = m_Phases[i].BlendDuration.ToMinutes();

            // Next phase time
            nextPhaseMinutes = TDayTime.MinutesBetween(m_Phases[i].BlendStartTime, m_Phases[followingPhase].BlendStartTime);

            // Case 1: current time falls between phase start and blend end --> blend should be executing
            if (currentMinutes < blendEndMinutes)
            {
                // current phase is then technically next phase
                m_NextPhaseIndex = i;

                // Start the blend from current time between previous phase and current phase
                StartCoroutine(Blend(RepeatIndex(i - 1, count), i, currentMinutes));
            }

            // Case 2: current time falls between blend end and next phase start time --> current phase is stable
            else if (currentMinutes < nextPhaseMinutes)
            {
                // following phase is then next phase
                m_NextPhaseIndex = followingPhase;

                // set current phase's lighting state
                m_MainCamera.backgroundColor = m_Phases[i].State.BackgroundColor;
                m_LightSource.color = m_Phases[i].State.LightColor;
            }

            // Case 3: current time falls in the following phase; check at next iteration
        }


    }

    /// <summary>
    /// Checks if next phase should start and eventually starts blending between phases.
    /// </summary>
    /// <param name="inCurrentTime"></param>
    private void CheckTimeAndStartBlend(TDayTime inCurrentTime)
    {
        if (inCurrentTime == m_Phases[m_NextPhaseIndex].BlendStartTime)
            StartCoroutine(Blend(RepeatIndex(m_NextPhaseIndex-1, m_Phases.Length), m_NextPhaseIndex));
    }

    /// <summary>
    /// Coroutine executing blending between phases.
    /// </summary>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    /// <param name="inStartMinute"></param>
    /// <returns></returns>
    private IEnumerator Blend(int fromIndex, int toIndex, int inStartMinute = 0)
    {
        // Blend is split in two parts: FROM-->MID and MID-->TO

        // Get half duration in minutes
        float halfDuration = m_Phases[toIndex].BlendDuration.ToMinutes() / 2.0f;
        
        // Get blend speed
        float speed = 0.5f / (halfDuration * TTimeManager.SharedInstance.TimeSettings.DayMinuteDuration);

        float secondHalfStart;

        // If the blend should starts in the first half
        if (inStartMinute < halfDuration)
        {
            // Second half starts at 0
            secondHalfStart = 0;

            // Execute first half
            for (float t = inStartMinute / halfDuration; t <= 1; t += speed * Time.deltaTime)
            {
                m_MainCamera.backgroundColor = Color.Lerp(m_Phases[fromIndex].State.BackgroundColor, m_Phases[toIndex].BlendState.BackgroundColor, t);
                m_LightSource.color = Color.Lerp(m_Phases[fromIndex].State.LightColor, m_Phases[toIndex].BlendState.LightColor, t);
                
                yield return null;
            }
        }
        else
        {
            // Otherwise, blend starts in second half, at percentage (startMinute - halfDuration) / halfDuration
            secondHalfStart = inStartMinute / halfDuration - 1;
        }

        // Execute second half
        for (float t = secondHalfStart; t <= 1; t += speed * Time.deltaTime)
        {
            m_MainCamera.backgroundColor = Color.Lerp(m_Phases[toIndex].BlendState.BackgroundColor, m_Phases[toIndex].State.BackgroundColor, t);
            m_LightSource.color = Color.Lerp(m_Phases[toIndex].BlendState.LightColor, m_Phases[toIndex].State.LightColor, t);
            
            yield return null;
        }

        // Advance next phase index
        m_NextPhaseIndex = RepeatIndex(m_NextPhaseIndex + 1, m_Phases.Length);

    }

    #endregion
}