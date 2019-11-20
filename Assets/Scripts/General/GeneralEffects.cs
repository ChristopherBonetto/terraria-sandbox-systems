using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralEffects
{
    public const float KbGlobalEffect = 50;

    /// <summary>
    /// Return the actual Knock back effect.
    /// </summary>
    /// <param name="inKbResist"></param>
    /// <returns></returns>
    public static float KbEffect(float inKbResist)
    {
        var percentage = (KbGlobalEffect * inKbResist) / 100;
        return KbGlobalEffect - percentage;
    }
}
