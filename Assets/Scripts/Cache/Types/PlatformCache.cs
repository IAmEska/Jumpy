using UnityEngine;
using System.Collections;

public class PlatformCache : AbstractCache<Platform> {

    public LevelGenerator levelGenerator;
  
    protected override void AdditionStart(Platform item)
    {
        item.levelGenerator = levelGenerator;
    }

    public override void Return(Platform obj)
    {
        obj.Reset();
        base.Return(obj);
    }

}
