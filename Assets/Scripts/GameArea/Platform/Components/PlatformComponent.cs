using UnityEngine;
using System.Collections;

public abstract class PlatformComponent : MonoBehaviour {

    protected Platform _platform;

    protected virtual void Awake()
    {
        _platform = GetComponent<Platform>();
    }
           
}
