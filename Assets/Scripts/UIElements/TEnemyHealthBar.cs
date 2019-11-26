using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Terraria.AI
{
    public class TEnemyHealthBar : MonoBehaviour
    {
        [SerializeField]
        private TEnemyDefenseComponent m_TargetHealth;
        public TEnemyDefenseComponent Targethealth => m_TargetHealth;

        [SerializeField]
        private Slider m_Slider;

        private Camera m_MainCamera;


        private void OnEnable()
        {
            TEventManager.SubscribeTo<IDefend>(TEventID.OnHealthUpdate, UpdateHealthBar);
        }

        private void OnDisable()
        {
            TEventManager.UnsubscribeFrom<IDefend>(TEventID.OnHealthUpdate, UpdateHealthBar);
        }

        private void Start()
        {
            m_MainCamera = Camera.main;
        }

        private void Update()
        {
            transform.position = m_MainCamera.WorldToScreenPoint(m_TargetHealth.transform.position - Vector3.up);
        }

        /// <summary>
        /// Reference target health.
        /// </summary>
        public void SetHealthReference(IDefend defenseComponent)
        {
            m_TargetHealth = (TEnemyDefenseComponent)defenseComponent;
        }

        /// <summary>
        /// Update health bar when some damage is taken.
        /// </summary>
        public virtual void UpdateHealthBar(IDefend defendComponent)
        {
            if (defendComponent == (IDefend)Targethealth)
            {
                m_Slider.maxValue = defendComponent.MaxHealth;
                m_Slider.value = defendComponent.CurrentHealth;
            }
        }
    }
}
