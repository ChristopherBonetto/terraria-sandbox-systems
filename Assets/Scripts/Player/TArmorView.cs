using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store all necessary visual armor part.
/// This class has no functions.
/// Variables are set in PlayerController.
/// </summary>
public class TArmorView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_ArmorRenderer;
    [SerializeField] private Animator m_ArmorAnim;

    [Header("Default Values")]
    [SerializeField] private Sprite m_DefaultIcon;
    [SerializeField] private RuntimeAnimatorController m_DefaultAnim;

    public SpriteRenderer ArmorRenderer
    {
        get { return m_ArmorRenderer; }
        set { m_ArmorRenderer = value; }
    }
    public Animator ArmorAnim
    {
        get { return m_ArmorAnim; }
        set { m_ArmorAnim = value; }
    }


    public void SetSprite(Sprite inIcon)
    {
        if (inIcon == null) return;
        m_ArmorRenderer.sprite = inIcon;
    }

    public void SetAnimator(AnimatorOverrideController inAnim)
    {
        if (inAnim == null) return;
        m_ArmorAnim.runtimeAnimatorController = inAnim;
    }

    /// <summary>
    /// restore the default values of this equipment (view only).
    /// </summary>
    public void ResteValues()
    {
        m_ArmorRenderer.sprite = m_DefaultIcon;
        m_ArmorAnim.runtimeAnimatorController = m_DefaultAnim;
    }

    public void SetAnim(string inName, bool inValue)
    {
        if (m_ArmorAnim == null) return;
        m_ArmorAnim.SetBool(inName, inValue);
    }
}
