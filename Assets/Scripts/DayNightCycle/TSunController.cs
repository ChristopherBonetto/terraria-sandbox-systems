using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSunController : MonoBehaviour
{
    public Transform TransformComponent { get; private set; }

    [SerializeField] private Vector2 m_EllipseSemiAxes;

    [SerializeField] private Transform m_Sun;
    [SerializeField] private Transform m_Moon;

    [SerializeField] private float m_Phase;

    private const float ANGLE_INCREMENT = Mathf.PI / TDayTime.HALF_DAY_MINUTES;

    private Vector3 m_Position;

    private float angleRad;

    private void Awake()
    {
        TransformComponent = transform;
    }

    private void Start()
    {
        m_Sun.gameObject.SetActive(true);
        m_Moon.gameObject.SetActive(false);

        m_Position.x = m_EllipseSemiAxes.x;

        m_Sun.localPosition = m_Position;
        m_Moon.localPosition = -m_Position;
    }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<int>(TEventID.OnMinutePassed, UpdatePositions);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<int>(TEventID.OnMinutePassed, UpdatePositions);
    }

    public void UpdatePositions(int inCurrentMinutes)
    {
        angleRad = inCurrentMinutes * ANGLE_INCREMENT + m_Phase * Mathf.Deg2Rad;

        m_Position.x = Mathf.Cos(angleRad) * m_EllipseSemiAxes.x;
        m_Position.y = Mathf.Sin(angleRad) * m_EllipseSemiAxes.y;

        if (m_Sun.gameObject.activeInHierarchy)
        {
            if (m_Position.y > 0)
                m_Sun.localPosition = m_Position;
            else
                SwitchActivation();
        }
        else
        {
            if (m_Position.y < 0)
                m_Moon.localPosition = -m_Position;
            else
                SwitchActivation();
        }
    }

    private void SwitchActivation()
    {
        m_Sun.gameObject.SetActive(!m_Sun.gameObject.activeInHierarchy);
        m_Moon.gameObject.SetActive(!m_Moon.gameObject.activeInHierarchy);
    }
}
