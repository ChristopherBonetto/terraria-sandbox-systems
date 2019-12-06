using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Terraria.AI
{
    public class TEnemyDefenseComponent : MonoBehaviour, IDefend
    {
        /// <summary>
        /// SerializeField
        /// </summary>

        [SerializeField] private TItemQuantity m_Containeditem;

        /// <summary>
        /// Private
        /// </summary>

        private TEnemyHealthBar m_HealthSlider; // Take it from the pool
        private float m_MaxHealth;
        private float m_Defense;
        private float m_CurrentHealth;
        private float m_LastHealth;

        /// <summary>
        /// Properties
        /// </summary>

        public float MaxHealth => m_MaxHealth;
        public float Defense => m_Defense;
        public float LastHealth => m_LastHealth;
        public float CurrentHealth
        {
            get { return m_CurrentHealth; }
            private set
            {
                var lastHealth = m_CurrentHealth;
                m_CurrentHealth = Mathf.Clamp(value, 0, MaxHealth);

                // Call UI event.
                TEventManager.TriggerEvent<IDefend>(TEventID.OnHealthUpdate, (IDefend)this);

                m_LastHealth = m_CurrentHealth;

                if (m_CurrentHealth <= 0)
                    DisposeToDead();

            }
        }


        /// <summary>
        /// Editor testing.
        /// </summary>
        [ContextMenu("Take Damage")]
        public void DamageEntity()
        {
            TakeDamage(1);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.RightControl))
                DamageEntity();
        }

        public void Init(float inMaxHealth)
        {
            m_MaxHealth = inMaxHealth;
            m_CurrentHealth = m_MaxHealth;
        }

        public void Init(float inMaxHealth, float inDefense)
        {
            m_MaxHealth = inMaxHealth;
            m_CurrentHealth = m_MaxHealth;
            m_Defense = inDefense;
        }

        public void TakeDamage(float inAmount)
        {
            if (m_HealthSlider == null)
            {
                // Pool UI slider health.
                m_HealthSlider = ObjectPooler.SharedInstance.GetPooledObject("HealthBar").GetComponent<TEnemyHealthBar>();

                // Set the defenseComponent of UI slider.
                m_HealthSlider.SetHealthReference(this);
                m_HealthSlider.gameObject.SetActive(true);
            }

            // Pool text where display the damage taken
            GameObject text = ObjectPooler.SharedInstance.GetPooledObject("DamageText");
            text.SetActive(true);
            text.transform.position = transform.position;

            if (!m_HealthSlider.gameObject.activeSelf)
                m_HealthSlider.gameObject.SetActive(true);

            m_LastHealth = CurrentHealth;
            CurrentHealth -= Mathf.Max(1, inAmount - Defense);
        }

        private void DisposeToDead()
        {
            // Turn off slider, 
            // spawn the dropped item, 
            // turn off this game object.

            m_HealthSlider.gameObject.SetActive(false);

            // Get Pickup object from the pool
            GameObject pickupObj = ObjectPooler.SharedInstance.GetPooledObject("Pickup");

            if (pickupObj)
            {
                // Load Item
                TItemPickup pickup = pickupObj.GetComponent<TItemPickup>();
                pickup.LoadItem(m_Containeditem);

                // Set Pickup position
                pickup.TransformComponent.position = transform.position;

                // Show Pickup
                pickupObj.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}
