using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{
	public Platform startPlatform;

	public EnemyFactory enemyFactory;
	public CollectibleFactory collectibleFactory;
	public PlatformFactory platformFactory;

	public bool generate = false;

	public float offsetY, offsetX;
	public float coinChance = 20;
	public float enemyChance = 5;
	public float minPlatformWidth, maxPlatformWidth;

	public int maxPlatformAtOnce, minPlatformAtOnce;

	protected float _startPosY, _currentPositionY;
	protected float _areaMinX, _areaMaxX;
	protected Platform _startPlatformRef;
	protected Platform _prevPlatform;
	protected bool _initStartPlatform = true;

	// Use this for initialization
	void Start ()
	{
		_startPosY = Camera.main.transform.position.y - Camera.main.orthographicSize;
		_currentPositionY = _startPosY + offsetY;

		float sizeX = Camera.main.orthographicSize * Screen.width / Screen.height;
		_areaMinX = Camera.main.transform.position.x - sizeX;
		_areaMaxX = Camera.main.transform.position.x + sizeX;

		platformFactory.SetBounds (_areaMinX, _areaMaxX);
		enemyFactory.SetBounds (_areaMinX, _areaMaxX);
		platformFactory.SetBounds (_areaMinX, _areaMaxX);
		platformFactory.SetOffset (offsetX);
	}

	public void Reset ()
	{
		_currentPositionY = _startPosY + offsetY;
		_initStartPlatform = true;
		generate = false;
		ClearAll ();
	}

	void FixedUpdate ()
	{
		if (_initStartPlatform) {
			platformFactory.InstantiateStartPlatform(_startPosY);
			_initStartPlatform = false;
		}

		if (generate) {
			if (_currentPositionY < Camera.main.transform.position.y + Camera.main.orthographicSize) {
				GeneratePlatform ();
				GenerateCoin ();
				GenerateEnemy ();
				_currentPositionY += offsetY;
			}
		}
	}

	protected void ClearAll ()
	{
		Clear (platformFactory);
		Clear (enemyFactory);
		Clear (collectibleFactory);
	}

	protected void Clear<T> (AbstractFactory<T> factory) where T : MonoBehaviour
	{
		if (factory.transform.childCount > 0) {
			for (int i=0; i< factory.transform.childCount; i++) {
				var t = factory.transform.GetChild (i);
				if(t is T)
				{
					factory.DestoryItem(t as T);
				}
			}
		}
	}

	protected void GeneratePlatform ()
	{
		platformFactory.InstantiateItem (0, _currentPositionY);
	}

	protected void GenerateCoin ()
	{
		int rP = Random.Range (0, 100);
		if (rP <= coinChance) {
			collectibleFactory.InstantiateItem(0, _currentPositionY + offsetY / 2);
		}
	}

	protected void GenerateEnemy ()
	{
		int rE = Random.Range (0, 100);
		if (rE <= enemyChance) {
			int eT = Random.Range (0, enemyFactory.items.Length);
			enemyFactory.InstantiateItem (eT, _currentPositionY + offsetY / 2);
		}
	}
}
