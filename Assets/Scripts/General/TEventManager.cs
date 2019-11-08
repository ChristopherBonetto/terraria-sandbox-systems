using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEventManager
{
    #region Static singleton reference

    /// <summary>
    /// Singleton instance reference.
    /// </summary>
    public static TEventManager SharedInstance
    {
        get
        {
            if (m_SharedInstance == null)
                m_SharedInstance = new TEventManager();

            return m_SharedInstance;
        }
    }

    private static TEventManager m_SharedInstance;

    #endregion

    #region Delegates definition

    // Timing
    public delegate void TDayPhaseEvent(bool isNight);

    // Generic
    public delegate void TCallbackEvent();

    #endregion

    #region Events

    // Items
    public event TCallbackEvent OnItemEquipped;

    // Day-night cycle
    public event TDayPhaseEvent OnDayPhaseChanged;

    #endregion


}
