using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    bool DepleteOnUse { get; }

    bool Use(TPlayerController inUser, TPointerData inData);
}
