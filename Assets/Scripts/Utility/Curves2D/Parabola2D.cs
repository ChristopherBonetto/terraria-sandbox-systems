using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Parabola 2D", menuName ="Curve 2D/Parabola")]
public class Parabola2D : Curve2D
{
    [Header("y = ax^2 + bx + c")]
    [SerializeField] private float a;
    [SerializeField] private float b;
    [SerializeField] private float c;

    public override float X(float t)
    {
        return t;
    }

    public override float Y(float t)
    {
        return a * t * t + b * t + c;
    }
}
