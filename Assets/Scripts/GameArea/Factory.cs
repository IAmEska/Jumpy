using UnityEngine;
using System.Collections;

public abstract class Factory : MonoBehaviour  {

    protected float _minX, _maxX;

    public virtual void SetBounds(float minX, float maxX)
    {
        _minX = minX;
        _maxX = maxX;
    }

    public abstract System.Type ItemType();

    public abstract void InstantiateItem(float positionY);

    public abstract void DestoryItem(Component item);

}
