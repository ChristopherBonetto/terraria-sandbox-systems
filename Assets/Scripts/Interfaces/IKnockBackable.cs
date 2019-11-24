using UnityEngine;

public interface IKnockBackable
{
    float KbResist { get; }

    void Freeze(float inTime);
    void KnockBack(Vector2 direction);
}
