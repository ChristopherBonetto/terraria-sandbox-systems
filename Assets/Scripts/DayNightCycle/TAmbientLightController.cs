using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

[RequireComponent(typeof(Light2D))]
public class TAmbientLightController : MonoBehaviour
{
    [SerializeField] private TLightingState m_DayLight;
    [SerializeField] private TLightingState m_NightLight;

    private Camera m_MainCamera;
    private Light2D m_LightSource;

    private float m_Percentage;

    private void Awake()
    {
        m_MainCamera = Camera.main;
        m_LightSource = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TDayTime>(TEventID.OnMinutePassed, UpdateView);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TDayTime>(TEventID.OnMinutePassed, UpdateView);
    }

    private void UpdateView(TDayTime inCurrentTime)
    {
        int inCurrentMinutes = inCurrentTime.ToMinutes();

        m_Percentage = inCurrentMinutes < TDayTime.HALF_DAY_MINUTES ? inCurrentMinutes / (float)TDayTime.HALF_DAY_MINUTES : (TDayTime.DAY_MINUTES - inCurrentMinutes) / (float)TDayTime.HALF_DAY_MINUTES;

        m_MainCamera.backgroundColor = Color.Lerp(m_NightLight.BackgroundColor, m_DayLight.BackgroundColor, m_Percentage);
        m_LightSource.color = Color.Lerp(m_NightLight.LightColor, m_DayLight.LightColor, m_Percentage);
    }
}
