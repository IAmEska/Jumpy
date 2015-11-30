using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Platform))]
[RequireComponent(typeof(BoxCollider2D))]
public class DestroyablePlatform : PlatformComponent
{  
    public enum DestroyableState
    {
        Idle,
        Shaking,
        Fall
    }

    public float shakeTime = 0.5f;
    protected DestroyableState _state = DestroyableState.Idle,
        _prevState = DestroyableState.Idle;

    protected override void Awake()
    {
        base.Awake();
        _state = DestroyableState.Idle;
        _prevState = DestroyableState.Idle;                                                    
        _platform.SetSprite(1);
    }           

    public virtual void StartFalling()
    {
        _state = DestroyableState.Shaking;    
    }

    void FixedUpdate()
    {
        switch(_state)
        {
            case DestroyableState.Shaking:
                //TODO Shake object
                if(_prevState != _state)
                {
                    StartCoroutine(FallAfterTime());
                }
                break;

            case DestroyableState.Fall:
                if (_prevState != _state)
                {
                    _platform.SetKinematic(false);
                }
                break;
        } 
        _prevState = _state;
    }

    IEnumerator FallAfterTime()
    {
        yield return new WaitForSeconds(shakeTime);
        _state = DestroyableState.Fall;
    }      
}
