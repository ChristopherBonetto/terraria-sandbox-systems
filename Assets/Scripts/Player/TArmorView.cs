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
}
