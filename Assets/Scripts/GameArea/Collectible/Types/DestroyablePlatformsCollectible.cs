using UnityEngine;
using System.Collections;

public class DestroyablePlatformsCollectible : AbstractCollectiblePlatforms {

    void Awake()
    {
        value = Random.Range(valueMin, valueMax);
        type = PlatformTypeChance.PlatformType.Destroyable;
    }

}
