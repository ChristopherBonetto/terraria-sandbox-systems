using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{

    public class TTimedDeactivator : MonoBehaviour
    {
        [SerializeField] private float m_Duration;

        public void OnEnable()
        {
            Invoke("Deactivate", m_Duration);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Stop()
        {
            CancelInvoke("Deactivate");
        }
    }
}
