using UnityEngine;
using System.Collections;

public class ResizeablePlatformsCollectible : AbstractCollectiblePlatforms {
    
    public Sprite positiveScaleSprite, negativeScaleSprite;


	void Awake()
    {
        value = Random.Range(0, 2); 
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        if (value == 0)
        {
            sp.sprite = positiveScaleSprite;
            sound = AudioManager.AudioSound.Resize;
            value = valueMax;
        }
        else
        {
            sp.sprite = negativeScaleSprite;
            sound = AudioManager.AudioSound.Shrink;
            value = valueMin;
        }
        type = PlatformTypeChance.PlatformType.Resizeable;              
    }
}
