using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Curve2D : ScriptableObject
{
    public abstract float X(float t);
    public abstract float Y(float t);
}
