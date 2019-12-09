using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class TCollectionText : MonoBehaviour
    {
        #region Public properties

        /// <summary>
        /// RectTransform component attached to the GameObject.
        /// </summary>
        public RectTransform RectTransformComponent { get; private set; }

        #endregion

        #region Private variables

        /// <summary>
        /// Text component attached to the GameObject.
        /// </summary>
        private Text m_TextComponent;

        /// <summary>
        /// Duration of the text.
        /// </summary>
        private float m_Duration;

        /// <summary>
        /// Quantity currently being shown by the text.
        /// </summary>
        private TItemQuantity m_ShownQuantity;

        #endregion

        #region MonoBehaviour cycle

        private void Awake()
        {
            RectTransformComponent = GetComponent<RectTransform>();
            m_TextComponent = GetComponent<Text>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Shows the Text, loading an Item Quantity.
        /// </summary>
        /// <param name="inQuantity">Quantity to be shown.</param>
        /// <param name="inDuration">Duration of the text.</param>
        public void Show(TItemQuantity inQuantity, float inDuration)
        {
            // Set shown quantity and text
            m_ShownQuantity = inQuantity;
            m_TextComponent.text = inQuantity.ToString();

            // Save duration
            m_Duration = inDuration;

            // Invoke Hide method after duration
            Invoke("Hide", inDuration);

            gameObject.SetActive(true);
        }

        /// <summary>
        /// Increments the shown amount by the provided value.
        /// </summary>
        /// <param name="inAmount"></param>
        public void IncrementAmount(int inAmount)
        {
            // Increment amount and update text
            m_ShownQuantity.Amount += inAmount;
            m_TextComponent.text = m_ShownQuantity.ToString();

            // Re-invoke Hide method
            CancelInvoke("Hide");
            Invoke("Hide", m_Duration);
        }

        /// <summary>
        /// Hides the Text.
        /// </summary>
        public void Hide()
        {
            TEventManager.TriggerEvent(TEventID.OnCollectionTextDisabled, m_ShownQuantity.Item);
            gameObject.SetActive(false);
        }

        #endregion
    }
}
