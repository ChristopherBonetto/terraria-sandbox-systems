using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSunMoonActions : MonoBehaviour
{
    [SerializeField] private float m_rotationSpeed = 0.5f;
    [SerializeField] private float m_degreesToRotate = 45;
    [SerializeField] private float m_destinationTime;

    private Quaternion m_startingRotation;
    private Quaternion m_destinationRotation;
    private Vector3 m_nextPosition = Vector3.zero;

    private bool m_isRotating = false;
    private float m_progressRotation = 0;

    private float m_timer;
    

    

    private void Start()
    {
        m_startingRotation = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_isRotating)
        {
            if (Timer(m_destinationTime))
            {
                RotationSystem(m_degreesToRotate);
            }
        }
        else
        {
            Rotation(m_destinationRotation);
        }
        
    }

    public void RotationSystem(float degreesToRotate)
    {
        if (!m_isRotating)
        {
            m_nextPosition.z = m_startingRotation.eulerAngles.z + degreesToRotate;
            m_destinationRotation = Quaternion.Euler(m_nextPosition);
            m_isRotating = true;
        }
    }

    public void Rotation(Quaternion desideredRotation)
    {

        m_progressRotation += m_rotationSpeed * Time.deltaTime;
        m_progressRotation = Mathf.Clamp01(m_progressRotation);

        transform.rotation = Quaternion.Slerp(m_startingRotation, desideredRotation, m_progressRotation);


        if (m_progressRotation == 1)
        {
            m_startingRotation = transform.rotation;
            m_isRotating = false;
            m_progressRotation = 0;
        }
    }

    public bool Timer(float inDestinationTime)
    {
        m_timer += Time.deltaTime;

        if(m_timer >= inDestinationTime)
        {
            m_timer = 0;
            return true;
        }
        return false;
    }
}
