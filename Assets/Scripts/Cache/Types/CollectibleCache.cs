using UnityEngine;
using System.Collections;

public class CollectibleCache : AbstractCache<Collectible> {
    public AudioManager audioManager;

    protected override void AdditionStart(Collectible item)
    {
        item.audioManager = audioManager;
        base.AdditionStart(item);
    }

}
