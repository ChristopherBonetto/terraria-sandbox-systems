using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSunMoonLockRotation : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}
