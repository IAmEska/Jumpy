using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public abstract class AbstractCollectibleManager<T>
{
    [SerializeField]
    public List<CollectibleHolder> list;

    public abstract void AddCollectible(T collectible,  float toPositionY);         

    public abstract void CheckCollectibles(float positionY);

    public abstract void Clear();

    [Serializable]
    public class CollectibleHolder
    {
        [SerializeField]
        public T item;

        [SerializeField]
        public float fromPositionY;

        [SerializeField]
        public float toPositionY;
    }
}
