using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPlayerView : MonoBehaviour
{
    private Vector3 m_Scale;

    private void Awake()
    {
        m_Scale = transform.localScale;
    }

    public void Flip(Vector2 inDirection)
    {
        transform.localScale = (Vector3)new Vector2(inDirection.x * m_Scale.x, m_Scale.y);
    }
}
