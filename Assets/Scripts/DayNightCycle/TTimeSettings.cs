using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Time Settings", menuName = "Time Settings")]
public class TTimeSettings : ScriptableObject
{
    public TDayTime DayStartTime { get { return m_DayStartTime; } }
    public TDayTime NightStartTime { get { return m_NightStartTime; } }

    public TDayTime GameStartTime { get { return m_GameStartTime; } }

    public float DayMinuteDuration { get { return m_DayMinuteDurationInSeconds; } }

    public bool IsStartingDuringNight { get { return GameStartTime < DayStartTime || GameStartTime > NightStartTime; } }

    [SerializeField] private TDayTime m_DayStartTime = new TDayTime(4, 30);
    [SerializeField] private TDayTime m_NightStartTime = new TDayTime(19, 30);

    [SerializeField] private TDayTime m_GameStartTime = new TDayTime(8, 15);
    [SerializeField] private float m_DayMinuteDurationInSeconds = 1;
}

[System.Serializable]
public struct TDayTime
{
    public const int DAY_MINUTES = 1440;
    public const int HALF_DAY_MINUTES = 720;

    public int Hours
    {
        get { return m_Hours; }
        set
        {
            int temp = value;

            if (temp > 0)
            {
                while (temp > 23)
                    temp -= 24;
            }
            else
            {
                while (temp < 0)
                    temp += 24;
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

            if (temp > 0)
            {
                while (temp > 59)
                {
                    temp -= 60;
                    hours++;
                }
            }
            else if (temp < 0)
            {
                while (temp < 0)
                {
                    temp += 60;
                    hours--;
                }
            }

            Hours += hours;
            m_Minutes = temp;
        }
    }

    [SerializeField] private int m_Hours;
    [SerializeField] private int m_Minutes;

    public override string ToString()
    {
        return ((Hours < 10) ? "0" + Hours.ToString() : Hours.ToString()) + ":" + ((Minutes < 10) ? "0" + Minutes.ToString() : Minutes.ToString());
    }

    public TDayTime(int inHours = 0, int inMinutes = 0)
    {
        m_Hours = Mathf.Min(inHours, 23);
        m_Minutes = Mathf.Min(inMinutes, 59);
    }


    public static int MinutesBetween(TDayTime time1, TDayTime time2)
    {
        int minutes1 = time1.ToMinutes();
        int minutes2 = time2.ToMinutes();

        if (minutes1 < minutes2)
            return minutes2 - minutes1;
        else
            return DAY_MINUTES - minutes1 + minutes2;
    }

    public int ToSeconds()
    {
        return (m_Hours * 60 + m_Minutes) * 60;
    }

    public int ToMinutes()
    {
        return m_Hours * 60 + m_Minutes;
    }

    public static TDayTime FromMinutes(int inMinutes)
    {
        int hours = 0;

        while(inMinutes > 59)
        {
            hours++;
            inMinutes -= 60;
        }

        return new TDayTime(hours, inMinutes);
    }

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
}
