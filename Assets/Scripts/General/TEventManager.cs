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

    // Item
    public delegate void TItemEvent(ItemScriptable inItem);
    
    // Timing
    public delegate void TDayPhaseEvent(bool isNight);

    #endregion

    #region Events

    // Items
    public event TItemEvent OnItemEquipped;

    // Day-night cycle
    public event TDayPhaseEvent OnDayPhaseChanged;

    #endregion


    #region Public methods

    public void InvokeOnItemEquipped(ItemScriptable inItem)
    {
        OnItemEquipped?.Invoke(inItem);
    }

    public void InvokeOnDayPhaseChanged(bool isNight)
    {
        OnDayPhaseChanged?.Invoke(isNight);
    }

    #endregion
}
