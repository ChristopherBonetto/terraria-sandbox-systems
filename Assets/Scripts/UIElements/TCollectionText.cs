using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class TCollectionText : MonoBehaviour
    {
        public RectTransform RectTransformComponent { get; private set; }

        public string Content
        {
            get { return m_TextComponent.text; }
            set { m_TextComponent.text = value; }
        }

        public Color TextColor
        {
            get { return m_TextComponent.color; }
            set { m_TextComponent.color = value; }
        }

        private Text m_TextComponent;

        private float m_Duration;

        private TItemQuantity m_ShownQuantity;

        private void Awake()
        {
            RectTransformComponent = GetComponent<RectTransform>();
            m_TextComponent = GetComponent<Text>();
        }

        public void Show(TItemQuantity inQuantity, float inDuration)
        {
            m_ShownQuantity = inQuantity;
            m_TextComponent.text = inQuantity.ToString();
            m_Duration = inDuration;

            Invoke("Hide", inDuration);
            gameObject.SetActive(true);
        }

        public void IncrementAmount(int inAmount)
        {
            m_ShownQuantity.Amount += inAmount;
            m_TextComponent.text = m_ShownQuantity.ToString();

            CancelInvoke("Hide");
            Invoke("Hide", m_Duration);
        }

        public void Hide()
        {
            TEventManager.TriggerEvent(TEventID.OnCollectionTextDisabled, m_ShownQuantity.Item);
            gameObject.SetActive(false);
        }

        public void Stop()
        {
            CancelInvoke("Hide");
        }
    }
}
