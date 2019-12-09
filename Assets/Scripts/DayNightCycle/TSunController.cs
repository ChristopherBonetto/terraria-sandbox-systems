using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSunController : MonoBehaviour
{
    #region Public properties

    /// <summary>
    /// Transform component attached to the GameObject.
    /// </summary>
    public Transform TransformComponent { get; private set; }

    #endregion

    #region Serialized variables

    [Header("Curve")]
    /// <summary>
    /// Curve on which the object will move.
    /// </summary>
    [SerializeField] private Curve2D m_Curve;

    /// <summary>
    /// Range in which the the curve should be ran across.
    /// </summary>
    [Tooltip("Range in which the curve should be ran across.")]
    [SerializeField] private float m_CurveInterval;

    [Header("Timing")]
    
    /// <summary>
    /// Movement start time.
    /// </summary>
    [SerializeField] private TDayTime m_StartTime;

    /// <summary>
    /// Movement duration.
    /// </summary>
    [SerializeField] private TDayTime m_Duration;

    [Header("References")]

    /// <summary>
    /// Object's sprite.
    /// </summary>
    [SerializeField] private GameObject m_Sprite;

    #endregion

    #region Private variables

    /// <summary>
    /// Auxiliary variable for Local Position setting.
    /// </summary>
    private Vector3 m_CurrentLocalPosition;

    #endregion

    #region MonoBehaviour cycle

    private void Awake()
    {
        // Cache transform component
        TransformComponent = transform;

        // Cache local position z component (which is ignored by movement)
        m_CurrentLocalPosition.z = TransformComponent.localPosition.z;
    }

    private void OnEnable()
    {
        // Subscribe to events
        TEventManager.SubscribeTo<TDayTime>(TEventID.OnMinutePassed, StartMovement);
        TEventManager.SubscribeTo<TDayTime>(TEventID.OnTimeStarted, InitPosition);
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        TEventManager.UnsubscribeFrom<TDayTime>(TEventID.OnMinutePassed, StartMovement);
        TEventManager.UnsubscribeFrom<TDayTime>(TEventID.OnTimeStarted, InitPosition);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Starts movement if current time is movement start time.
    /// </summary>
    /// <param name="inCurrentTime"></param>
    private void StartMovement(TDayTime inCurrentTime)
    {
        if (inCurrentTime == m_StartTime) StartCoroutine(Move());
    }

    /// <summary>
    /// Initializes position on startup.
    /// </summary>
    /// <param name="inCurrentTime"></param>
    private void InitPosition(TDayTime inCurrentTime)
    {
        // Get durations in minutes
        int minutesBetween = TDayTime.MinutesBetween(m_StartTime, inCurrentTime);
        int durationMinutes = m_Duration.ToMinutes();

        // if minutes between start time and current time are less than duration, then the object should be moving
        if (minutesBetween < durationMinutes)
            StartCoroutine(Move(minutesBetween));
    }

    /// <summary>
    /// Moves the object along the set curve.
    /// </summary>
    /// <param name="startingMinute">Minute at which to start.</param>
    /// <returns></returns>
    private IEnumerator Move(int startingMinute = 0)
    {
        // Show sprite
        m_Sprite.SetActive(true);

        // Calculate speed
        float speed = 2 * m_CurveInterval / (m_Duration.ToMinutes() * TTimeManager.SharedInstance.TimeSettings.DayMinuteDuration);

        // Execute movement
        for (float t = -m_CurveInterval + startingMinute * 2 * m_CurveInterval / m_Duration.ToMinutes(); t <= m_CurveInterval; t += speed * Time.deltaTime)
        {
            m_CurrentLocalPosition.x = m_Curve.X(t);
            m_CurrentLocalPosition.y = m_Curve.Y(t);
            TransformComponent.localPosition = m_CurrentLocalPosition;
            
            yield return null;
        }

        // At the end, hide sprite
        m_Sprite.SetActive(false);
    }

    #endregion
}
