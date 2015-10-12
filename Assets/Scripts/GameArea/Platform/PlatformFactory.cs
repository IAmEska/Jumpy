using UnityEngine;
using System.Collections;

public class PlatformFactory : AbstractFactory<Platform> {

	public StartPlatform startPlatform;
    public float movingPlatformChance = 15f;
    public float destroyablePlatformChance = 10f;
    public bool isAdvancedPlatforms = false;

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

    public override void InstantiateItem(float positionY)
    {    
        _itemPosition = 0;
        _positionY = positionY;
    }

    #region implemented abstract members of AbstractFactory
    protected override void SpawnItem (Platform p)
	{

        p.SetSprite(0);
        if (isAdvancedPlatforms)
        { 
            float movingChance = Random.Range(0, 100);
            if(movingChance <= movingPlatformChance)
            {
                p.gameObject.AddComponent<MovingPlatform>();    
            } 


            float destroyableChance = Random.Range(0, 100);
            if (destroyableChance <= destroyablePlatformChance)
            {
                p.gameObject.AddComponent<DestroyablePlatform>();
                p.SetSprite(1);
            }
        }

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