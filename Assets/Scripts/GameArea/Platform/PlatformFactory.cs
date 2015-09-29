using UnityEngine;
using System.Collections;

public class PlatformFactory : AbstractFactory<Platform> {

	public StartPlatform startPlatform;
    

	protected float _offsetX;
	protected Platform _prevPlatform;


	public void SetOffset(float offsetX)
	{
		_offsetX = offsetX;
	}

	public StartPlatform InstantiateStartPlatform(float starPosY)
	{
		var sp = Instantiate<StartPlatform> (startPlatform);
		sp.transform.position = new Vector3 (0, starPosY, 0);
		sp.transform.parent = transform;
        return sp;
	}

    public override void SetBounds(float minX, float maxX)
    {
        base.SetBounds(minX, maxX);
        MovingPlatform.maxX = maxX;
        MovingPlatform.minX = minX;
    }

    #region implemented abstract members of AbstractFactory
    protected override void SpawnItem (Platform p)
	{
        //Platform p = cache.Get(_itemPosition);// Instantiate<Platform> (cache.cacheTypes[_itemPosition]);
		float posX = Random.Range (_minX, _maxX);
		if (_prevPlatform != null) {
			if (Mathf.Abs (posX - _prevPlatform.transform.position.x) < _offsetX) {
				int r = Random.Range (0, 2);
				if (r == 0) {
					posX -= _offsetX;
					if (posX < _minX)
						posX = _maxX - Mathf.Abs (posX);
				} else {
					posX += _offsetX;
					if (posX > _maxX)
						posX = _minX + Mathf.Abs (posX);
				}
			}
		}
		p.transform.localScale = new Vector3 (Random.Range (p.minWidth, p.maxWidth), 1, 1);
		p.transform.position = new Vector3 (posX, _positionY);
		_prevPlatform = p;
	}

	#endregion
}