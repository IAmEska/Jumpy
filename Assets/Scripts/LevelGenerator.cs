using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{
	public Platform platform;
	public Platform startPlatform;

	public EnemyFactory enemyFactory;

	public Coin coin;
	public bool generate = false;

	public GameObject platformHolder, coinHolder;

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
			_startPlatformRef = Instantiate<Platform> (startPlatform);
			_startPlatformRef.transform.position = new Vector3 (0, _startPosY, 0);
			_startPlatformRef.transform.parent = platformHolder.transform;
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
		Clear (platformHolder);
		Clear (enemyFactory.gameObject);
		Clear (coinHolder);
	}

	protected void Clear (GameObject go)
	{
		if (go.transform.childCount > 0) {
			for (int i=0; i< go.transform.childCount; i++) {
				var t = go.transform.GetChild (i);
				//TODO cache
				Destroy (t.gameObject);
			}
		}
	}

	protected void GeneratePlatform ()
	{
		//TODO cache
		Platform p = Instantiate<Platform> (platform);
		float posX = Random.Range (_areaMinX, _areaMaxX);
		if (_prevPlatform != null) {
			if (Mathf.Abs (posX - _prevPlatform.transform.position.x) < offsetX) {
				int r = Random.Range (0, 2);
				if (r == 0) {
					posX -= offsetX;
					if (posX < _areaMinX)
						posX = _areaMaxX - Mathf.Abs (posX);
				} else {
					posX += offsetX;
					if (posX > _areaMaxX)
						posX = _areaMinX + Mathf.Abs (posX);
				}
			}
		}
		p.transform.localScale = new Vector3 (Random.Range (minPlatformWidth, maxPlatformWidth), 1, 1);
		p.transform.position = new Vector3 (posX, _currentPositionY);
		p.transform.parent = platformHolder.transform;
		_prevPlatform = p;
	}

	protected void GenerateCoin ()
	{
		//TODO cache
		int rP = Random.Range (0, 100);
		if (rP <= coinChance) {
			Coin c = Instantiate<Coin> (coin);
			c.transform.position = new Vector3 (Random.Range (_areaMinX, _areaMaxX), _currentPositionY + offsetY / 2);
			c.transform.parent = coinHolder.transform;
		}
	}

	protected void GenerateEnemy ()
	{
		//TODO cache
		int rE = Random.Range (0, 100);
		if (rE <= enemyChance) {
			int eT = Random.Range (0, enemyFactory.enemies.Length);
			enemyFactory.InstantiateEnemy (eT, _currentPositionY + offsetY / 2, new Vector2 (_areaMinX, _areaMaxX));
		}
	}
}
