using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public abstract class AbstractCollectibleManager<T>
{
    [SerializeField]
    public List<CollectibleHolder<T>> list;

    public abstract void AddCollectible(T collectible,  int toPositionY);         

    public abstract void CheckCollectibles(float positionY);

    public abstract void Clear();

    [Serializable]
    public class CollectibleHolder<T>
    {
        [SerializeField]
        public T item;

        [SerializeField]
        public int fromPositionY;

        [SerializeField]
        public int toPositionY;
    }
}
