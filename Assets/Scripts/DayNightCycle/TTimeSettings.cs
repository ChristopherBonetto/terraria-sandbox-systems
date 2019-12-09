using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Time Settings", menuName = "Time Settings")]
public class TTimeSettings : ScriptableObject
{
    #region Public properties

    /// <summary>
    /// Time at which Day starts.
    /// </summary>
    public TDayTime DayStartTime { get { return m_DayStartTime; } }

    /// <summary>
    /// Time at which Night starts.
    /// </summary>
    public TDayTime NightStartTime { get { return m_NightStartTime; } }

    /// <summary>
    /// Time set at game startup.
    /// </summary>
    public TDayTime GameStartTime { get { return m_GameStartTime; } }

    /// <summary>
    /// Duration of a game minute in realtime seconds.
    /// </summary>
    public float DayMinuteDuration { get { return m_DayMinuteDurationInSeconds; } }

    /// <summary>
    /// Whether the game starts during night.
    /// </summary>
    public bool IsStartingDuringNight { get { return GameStartTime < DayStartTime || GameStartTime > NightStartTime; } }

    #endregion

    #region Serialized variables

    [SerializeField] private TDayTime m_DayStartTime = new TDayTime(4, 30);
    [SerializeField] private TDayTime m_NightStartTime = new TDayTime(19, 30);

    [Space]

    [SerializeField] private TDayTime m_GameStartTime = new TDayTime(8, 15);

    [Space]

    [SerializeField] private float m_DayMinuteDurationInSeconds = 1;

    #endregion
}

/// <summary>
/// Struct representing Day Time in HH:MM format.
/// </summary>
[System.Serializable]
public struct TDayTime
{
    #region Constants

    public const int HOURS_IN_DAY = 24;
    public const int MINUTES_IN_HOUR = 60;
    public const int DAY_MINUTES = HOURS_IN_DAY * MINUTES_IN_HOUR;

    #endregion

    #region Public properties

    public int Hours
    {
        get { return m_Hours; }
        set
        {
            int temp = value;

            // Make hours wrap between 0 and HOURS_IN_DAY
            if (temp > 0)
            {
                while (temp > HOURS_IN_DAY - 1)
                    temp -= HOURS_IN_DAY;
            }
            else
            {
                while (temp < 0)
                    temp += HOURS_IN_DAY;
            }

            m_Hours = temp;
        }
    }

    public int Minutes
    {
        get { return m_Minutes; }
        set
        {
            int temp = value;
            int hours = 0;

            // Make minutes wrap between 0 and MINUTES_IN_HOUR
            if (temp > 0)
            {
                while (temp > MINUTES_IN_HOUR-1)
                {
                    temp -= MINUTES_IN_HOUR;
                    hours++;
                }
            }
            else if (temp < 0)
            {
                while (temp < 0)
                {
                    temp += MINUTES_IN_HOUR;
                    hours--;
                }
            }

            Hours += hours;
            m_Minutes = temp;
        }
    }

    #endregion

    #region Serialized variables

    [SerializeField] private int m_Hours;
    [SerializeField] private int m_Minutes;

    #endregion

    #region Constructor

    public TDayTime(int inHours = 0, int inMinutes = 0)
    {
        m_Hours = Mathf.Min(inHours, HOURS_IN_DAY-1);
        m_Minutes = Mathf.Min(inMinutes, MINUTES_IN_HOUR-1);
    }

    public TDayTime(int inMinutes = 0)
    {
        m_Hours = 0;

        while (inMinutes > MINUTES_IN_HOUR - 1)
        {
            m_Hours++;
            inMinutes -= MINUTES_IN_HOUR;
        }

        while (m_Hours > HOURS_IN_DAY - 1)
            m_Hours -= HOURS_IN_DAY;

        m_Minutes = inMinutes;
    }

    #endregion


    #region Static methods

    /// <summary>
    /// Calculates minutes between two times.
    /// </summary>
    /// <param name="time1"></param>
    /// <param name="time2"></param>
    /// <returns></returns>
    public static int MinutesBetween(TDayTime time1, TDayTime time2)
    {
        int minutes1 = time1.ToMinutes();
        int minutes2 = time2.ToMinutes();

        if (minutes1 < minutes2)
            return minutes2 - minutes1;
        else
            return DAY_MINUTES - minutes1 + minutes2;
    }

    /// <summary>
    /// Creates a time from minutes input.
    /// </summary>
    /// <param name="inMinutes"></param>
    /// <returns></returns>
    public static TDayTime FromMinutes(int inMinutes)
    {
        int hours = 0;

        while (inMinutes > MINUTES_IN_HOUR - 1)
        {
            hours++;
            inMinutes -= MINUTES_IN_HOUR;
        }

        return new TDayTime(hours, inMinutes);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Converts the time in HH:MM format to minutes.
    /// </summary>
    /// <returns></returns>
    public int ToMinutes()
    {
        return m_Hours * MINUTES_IN_HOUR + m_Minutes;
    }

    #endregion

    #region Operators and defaults overrides

    public static bool operator ==(TDayTime a, TDayTime b)
    {
        return a.Hours == b.Hours && a.Minutes == b.Minutes;
    }

    public static bool operator !=(TDayTime a, TDayTime b)
    {
        return a.Hours != b.Hours || a.Minutes != b.Minutes;
    }

    public static bool operator >(TDayTime a, TDayTime b)
    {
        return a.ToMinutes() > b.ToMinutes();
    }

    public static bool operator <(TDayTime a, TDayTime b)
    {
        return a.ToMinutes() < b.ToMinutes();
    }

    public static TDayTime operator -(TDayTime a, TDayTime b)
    {
        int diff = a.ToMinutes() - b.ToMinutes();

        if (diff < 0) diff = DAY_MINUTES - diff;

        return FromMinutes(diff);
    }

    public static TDayTime operator +(TDayTime a, TDayTime b)
    {
        int sum = a.ToMinutes() + b.ToMinutes();

        if (sum > DAY_MINUTES) sum -= DAY_MINUTES;

        return FromMinutes(sum);
    }

    public static TDayTime operator /(TDayTime a, float b)
    {
        return FromMinutes(Mathf.RoundToInt(a.ToMinutes() / b));
    }

    public override string ToString()
    {
        return ((Hours < 10) ? "0" + Hours.ToString() : Hours.ToString()) + ":" + ((Minutes < 10) ? "0" + Minutes.ToString() : Minutes.ToString());
    }

    #endregion
}
