using UnityEngine;
using System.Collections;


public class SelfDestroyPlatform : DestroyablePlatform
{
    public float minCountdown = 1f;
    public float maxCountdown = 3f;

    protected override void Awake()
    {                              
        base.Awake();
        shakeTime = Random.Range(minCountdown, maxCountdown);
        _state = DestroyableState.Shaking;
        _prevState = DestroyableState.Idle;
        _platform.SetSprite(2);
    }

    public override void StartFalling()
    {
        //_state = DestroyableState.Shaking;
    }
}
