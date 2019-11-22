using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class TPointerFollowerUI : MonoBehaviour
    {
        public RectTransform RectTransformComponent { get; private set; }

        [SerializeField] private Image m_ItemImage;

        private void Awake()
        {
            RectTransformComponent = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemSelected, StartFollowing);
            TEventManager.SubscribeTo(TEventID.OnItemDeselected, StopFollowing);
        }

        private void OnDisable()
        {
            TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemSelected, StartFollowing);
            TEventManager.UnsubscribeFrom(TEventID.OnItemDeselected, StopFollowing);
        }

        private void StartFollowing(TInventorySlot inSlot)
        {
            m_ItemImage.sprite = inSlot.ItemInSlot.Item.ItemSprite;
            m_ItemImage.gameObject.SetActive(true);
            StartCoroutine("FollowPointer");
        }

        private void StopFollowing()
        {
            m_ItemImage.gameObject.SetActive(false);
            StopCoroutine("FollowPointer");
        }

        private IEnumerator FollowPointer()
        {
            while (Application.isPlaying)
            {
                RectTransformComponent.position = Input.mousePosition;
                yield return null;
            }
        }
    }
}
