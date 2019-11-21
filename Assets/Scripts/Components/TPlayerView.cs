using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TPlayerView : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField]
    private Animator m_Anim;
    /// <summary>
    /// Player's animator.
    /// </summary>
    public Animator Anim { get { return m_Anim; } }

    [SerializeField]
    private GameObject m_ItemRootAnimation;

    private void OnEnable()
    {
        TEventManager.SubscribeTo<TPointerData>(TEventID.OnLeftClickDown, PlayerAttackAnimation);
    }

    private void OnDisable()
    {
        TEventManager.UnsubscribeFrom<TPointerData>(TEventID.OnLeftClickDown, PlayerAttackAnimation);
    }

    private void PlayerAttackAnimation(TPointerData pointerData)
    {
        if (!m_ItemRootAnimation.activeSelf)
        {
            m_ItemRootAnimation.SetActive(true);
            Anim.SetTrigger("Attack");
        }
    }

    public void TurnOffItemRoot()
    {
        m_ItemRootAnimation.SetActive(false);
    }
}
