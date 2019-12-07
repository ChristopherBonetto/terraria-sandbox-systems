using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSunController : MonoBehaviour
{
    public Transform TransformComponent { get; private set; }

    [SerializeField] private Curve2D m_Curve;

    [SerializeField] private TDayTime m_StartTime;
    [SerializeField] private TDayTime m_Duration;
    [SerializeField] private float m_CurveInterval;

    [SerializeField] private GameObject m_Sprite;

    private Vector3 m_CurrentPosition;

    private void Awake()
    {
        TransformComponent = transform;

        m_CurrentPosition.z = TransformComponent.position.z;
    }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TDayTime>(TEventID.OnMinutePassed, StartMovement);
        TEventManager.SubscribeTo<TDayTime>(TEventID.OnTimeStarted, InitPosition);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TDayTime>(TEventID.OnMinutePassed, StartMovement);
        TEventManager.UnsubscribeFrom<TDayTime>(TEventID.OnTimeStarted, InitPosition);
    }

    private void StartMovement(TDayTime inCurrentTime)
    {
        if (inCurrentTime == m_StartTime) StartCoroutine(Move());
    }

    private void InitPosition(TDayTime inCurrentTime)
    {
        int minutesBetween = TDayTime.MinutesBetween(m_StartTime, inCurrentTime);
        int durationMinutes = m_Duration.ToMinutes();

        if (minutesBetween < durationMinutes)
        {
            StartCoroutine(Move(minutesBetween));
        }
    }

    private IEnumerator Move(int startingMinute = 0)
    {
        m_Sprite.SetActive(true);

        float speed = 2 * m_CurveInterval / (m_Duration.ToMinutes() * TTimeManager.SharedInstance.TimeSettings.DayMinuteDuration);

        for (float t = -m_CurveInterval + startingMinute * 2 * m_CurveInterval / m_Duration.ToMinutes(); t <= m_CurveInterval; t += speed * Time.deltaTime)
        {
            m_CurrentPosition.x = m_Curve.X(t);
            m_CurrentPosition.y = m_Curve.Y(t);
            TransformComponent.localPosition = m_CurrentPosition;
            
            yield return null;
        }

        m_Sprite.SetActive(false);
    }
}
