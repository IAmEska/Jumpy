using UnityEngine;
using System.Collections;

public class PlatformFactory : AbstractFactory<Platform> {

	public Platform startPlatform;

	protected float _offsetX;
	protected Platform _prevPlatform;


	public void SetOffset(float offsetX)
	{
		_offsetX = offsetX;
	}

	public void InstantiateStartPlatform(float starPosY)
	{
		var sp = Instantiate<Platform> (startPlatform);
		sp.transform.position = new Vector3 (0, starPosY, 0);
		sp.transform.parent = transform;
	}

	#region implemented abstract members of AbstractFactory
	protected override void SpawnItem ()
	{
		Platform p = Instantiate<Platform> (items[_itemPosition]);
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
		p.transform.parent = transform;
		_prevPlatform = p;
	}
	#endregion
}