using UnityEngine;
using System.Collections;

public class MovingPlatformsCollectible : AbstractCollectiblePlatforms {

	void Awake()
    {
        type = PlatformTypeChance.PlatformType.MovingPlatform;
        value = Random.Range(valueMin, valueMax);
    }
}
