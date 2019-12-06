using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    bool Use(TPlayerController inUser, TPointerData inData);
}
