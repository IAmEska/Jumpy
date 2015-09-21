using UnityEngine;
using System.Collections;

public abstract class AbstractFactory<T> : MonoBehaviour {

	public T[] items;
	
	protected int _itemPosition;
	protected float _minX, _maxX;
	protected float _positionY;

	void Start () {
		_itemPosition = -1;
	}

	void FixedUpdate () {
		if (_itemPosition > 0) {
			SpawnItem();
		}
		_itemPosition = -1;
	}

	public void SetBounds(float minX, float maxX)
	{
		_minX = minX;
		_maxX = maxX;
	}

	public void InstantiateItem(int itemPosition, float positionY)
	{
		_itemPosition = itemPosition;
		_positionY = positionY;
	}

	protected abstract void SpawnItem();
}
