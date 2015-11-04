﻿using UnityEngine;
using System.Collections;

public abstract class AbstractFactory<T> : Factory where T : MonoBehaviour {

    public AbstractCache<T> cache;

    protected int _itemPosition;
	
	protected float _positionY;

    public override System.Type ItemType()
    {
        return typeof(T);
    }

	void Awake () {
        cache = GetComponentInChildren<AbstractCache<T>>();
        _itemPosition = -1;
	}

	void FixedUpdate () {
		if (_itemPosition >= 0 && _itemPosition < cache.cacheTypes.Length) {
            T item = cache.Get(_itemPosition);
            item.gameObject.SetActive(true);
            item.transform.parent = transform;
			SpawnItem(item);
		}
		_itemPosition = -1;
	}

	

    public abstract override void InstantiateItem(float positionY);

	public override void DestoryItem(Component item)
    {
        cache.Return(item as T);
    }

	protected abstract void SpawnItem(T item);
}
