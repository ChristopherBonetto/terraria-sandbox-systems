using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEventManager
{
    public delegate void Callback();
    public delegate void Callback<T>(T arg);
    public delegate void Callback<T, K>(T arg1, K arg2);

    private static Dictionary<TEventID, Delegate> m_Events = new Dictionary<TEventID, Delegate>();

    public static void SubscribeTo(TEventID inEvent, Callback inHandler)
    {
        if (!m_Events.ContainsKey(inEvent)) m_Events.Add(inEvent, null);

        if (m_Events[inEvent] != null)
        {
            Delegate[] invocationList = ((Callback)m_Events[inEvent]).GetInvocationList();

            for (int i = 0; i < invocationList.Length; i++)
            {
                if ((Delegate)inHandler == invocationList[i])
                    return;
            }
        }

        m_Events[inEvent] = (Callback)m_Events[inEvent] + inHandler;
    }

    public static void SubscribeTo<T>(TEventID inEvent, Callback<T> inHandler)
    {
        if (!m_Events.ContainsKey(inEvent)) m_Events.Add(inEvent, null);

        if (m_Events[inEvent] != null)
        {
            Delegate[] invocationList = ((Callback<T>)m_Events[inEvent]).GetInvocationList();

            for (int i = 0; i < invocationList.Length; i++)
            {
                if ((Delegate)inHandler == invocationList[i])
                    return;
            }
        }

        m_Events[inEvent] = (Callback<T>)m_Events[inEvent] + inHandler;
    }

    public static void SubscribeTo<T,K>(TEventID inEvent, Callback<T,K> inHandler)
    {
        if (!m_Events.ContainsKey(inEvent)) m_Events.Add(inEvent, null);

        if (m_Events[inEvent] != null)
        {
            Delegate[] invocationList = ((Callback<T,K>)m_Events[inEvent]).GetInvocationList();

            for (int i = 0; i < invocationList.Length; i++)
            {
                if ((Delegate)inHandler == invocationList[i])
                    return;
            }
        }

        m_Events[inEvent] = (Callback<T,K>)m_Events[inEvent] + inHandler;
    }


    public static void UnsubscribeFrom(TEventID inEvent, Callback inHandler)
    {
        if (m_Events.ContainsKey(inEvent))
        {
            m_Events[inEvent] = (Callback)m_Events[inEvent] - inHandler;

            if (m_Events[inEvent] == null) m_Events.Remove(inEvent);
        }
    }

    public static void UnsubscribeFrom<T>(TEventID inEvent, Callback<T> inHandler)
    {
        if (m_Events.ContainsKey(inEvent))
        {
            m_Events[inEvent] = (Callback<T>)m_Events[inEvent] - inHandler;

            if (m_Events[inEvent] == null) m_Events.Remove(inEvent);
        }
    }

    public static void UnsubscribeFrom<T,K>(TEventID inEvent, Callback<T,K> inHandler)
    {
        if (m_Events.ContainsKey(inEvent))
        {
            m_Events[inEvent] = (Callback<T,K>)m_Events[inEvent] - inHandler;

            if (m_Events[inEvent] == null) m_Events.Remove(inEvent);
        }
    }


    public static void TriggerEvent(TEventID inEvent)
    {
        if (m_Events.ContainsKey(inEvent)) (m_Events[inEvent] as Callback)?.Invoke();
    }

    public static void TriggerEvent<T>(TEventID inEvent, T arg)
    {
        if (m_Events.ContainsKey(inEvent)) (m_Events[inEvent] as Callback<T>)?.Invoke(arg);
    }

    public static void TriggerEvent<T,K>(TEventID inEvent, T arg1, K arg2)
    {
        if (m_Events.ContainsKey(inEvent)) (m_Events[inEvent] as Callback<T,K>)?.Invoke(arg1, arg2);
    }

    public static bool Exists(TEventID inEvent)
    {
        return m_Events.ContainsKey(inEvent);
    }
}
