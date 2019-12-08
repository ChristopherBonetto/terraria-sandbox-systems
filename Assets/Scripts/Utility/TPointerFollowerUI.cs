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

        #region Events

        private void OnEnable()
        {
            TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemSelected, StartFollowing);
            TEventManager.SubscribeTo<TInventorySlot>(TEventID.OnItemDeselected, StopFollowing);
        }

        private void OnDisable()
        {
            TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemSelected, StartFollowing);
            TEventManager.UnsubscribeFrom<TInventorySlot>(TEventID.OnItemDeselected, StopFollowing);
        }

        #endregion

        #region Move image in mouse position

        /// <summary>
        /// <param OnItemSelected> start a coroutine that it tell to this item to follow the mouse position.
        /// <param OnItemDeselected> stop the coroutine that it tell to this item to don't follow the mouse position.
        /// </summary>

        private void StartFollowing(TInventorySlot inSlot)
        {
            m_ItemImage.sprite = inSlot.ItemInSlot.Item.ItemSprite;
            m_ItemImage.gameObject.SetActive(true);
            StartCoroutine("FollowPointer");
        }

        private void StopFollowing(TInventorySlot inSlot)
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

        #endregion
    }
}
