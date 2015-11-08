using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlatformTypeChance  {

    public enum PlatformType
    {
        Destroyable,
        MovingPlatform,
        Resizeable
    }                               
                         
    public PlatformType type;
    
    public System.Type GetComponent()
    {
        switch (type)
        {
            default:
            case PlatformType.Destroyable:
                return typeof(DestroyablePlatform);  

            case PlatformType.MovingPlatform:
                return typeof(MovingPlatform);  
        }
    }

    [Range(0,100)]       
    public float chance;

    [HideInInspector]
    public float defualtChance;

    [HideInInspector]
    public int defaultMinimumSameComponentGap;

    //[HideInInspector]                  
    public float pseudoChance;

    [Range(0,1000)]
    public int minimumSameComponentGap;

    //[HideInInspector]
    public float lastSpawnPositionY;

}
