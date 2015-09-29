using UnityEngine;
using System.Collections;

public abstract class AbstractFactory<T> : MonoBehaviour where T : MonoBehaviour {

    public AbstractCache<T> cache;

    protected int _itemPosition;
	protected float _minX, _maxX;
	protected float _positionY;

	void Start () {
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

	public virtual void SetBounds(float minX, float maxX)
	{
		_minX = minX;
		_maxX = maxX;
	}

	public void InstantiateItem(int itemPosition, float positionY)
	{
		_itemPosition = itemPosition;
		_positionY = positionY;
	}

	public void DestoryItem(T item)
    {
        cache.Return(item);
    }

	protected abstract void SpawnItem(T item);
}
