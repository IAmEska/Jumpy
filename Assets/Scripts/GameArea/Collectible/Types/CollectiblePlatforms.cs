using UnityEngine;
using System.Collections;

public class CollectiblePlatforms : Collectible {

    [SerializeField]
    protected int valueMax = 20;

    [SerializeField]
    protected int valueMin = 10;

    public PlatformTypeChance.PlatformType type;

    public int value;

    void Awake()
    {
        value = Random.Range(valueMin, valueMax);
    }

    
	
}
