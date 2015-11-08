using UnityEngine;
using System.Collections;

public class AbstractCollectiblePlatforms : Collectible {

    [SerializeField]
    protected float valueMax = 20;

    [SerializeField]
    protected float valueMin = 10;

    public PlatformTypeChance.PlatformType type;

    public float value;


    
	
}
