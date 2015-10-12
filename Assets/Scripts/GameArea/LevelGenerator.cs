using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{

	public EnemyFactory enemyFactory;
	public CollectibleFactory collectibleFactory;
	public PlatformFactory platformFactory;

	public bool generate = false;

	public float offsetY, offsetX;
	public float coinChance = 20;
	public float enemyChance = 5;
    public float movingPlatformChance = 50f;
	public float minPlatformWidth, maxPlatformWidth;

	public int maxPlatformAtOnce, minPlatformAtOnce;

	protected float _startPosY, _currentPositionY;
	protected float _areaMinX, _areaMaxX;
	protected StartPlatform _startPlatformRef;

    protected bool _initStartPlatform, _isFirstRun;

	// Use this for initialization
	void Start ()
	{
        _initStartPlatform = true;
        _isFirstRun = true;

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
        generate = false;
        if (!_isFirstRun)
        { 
		    _initStartPlatform = true;
            ClearAll();
        }
        else
        {
            _isFirstRun = false;
        }
              
        
	}

	void FixedUpdate ()
	{
		if (_initStartPlatform) {
            _startPlatformRef = platformFactory.InstantiateStartPlatform(_startPosY);
			_initStartPlatform = false;
		}

		if (generate) {
			if (_currentPositionY < Camera.main.transform.position.y + Camera.main.orthographicSize) {
				GeneratePlatform ();
                if (_currentPositionY > _startPosY + 2 * Camera.main.orthographicSize)
                {
                    if (!platformFactory.isAdvancedPlatforms)
                        platformFactory.isAdvancedPlatforms = true;

                    GenerateCoin ();
				    GenerateEnemy ();

                }
                _currentPositionY += offsetY;
			}
		}
	}

    public void SetDeadStatus()
    {                           
        if(_startPlatformRef != null)
            _startPlatformRef.GetComponent<Collider2D>().isTrigger = true;

        platformFactory.isAdvancedPlatforms = false;
    }

    public void DestroyObject(GameObject item)
    {
        Enemy e = item.GetComponent<Enemy>();
        if(e != null)
        {
            enemyFactory.DestoryItem(e);
        }
        else
        {
            Platform p = item.GetComponent<Platform>();
            if(p != null)
            {
                platformFactory.DestoryItem(p);
            }
            else
            {
                Collectible c = item.GetComponent<Collectible>();
                if(c != null)
                {
                    collectibleFactory.DestoryItem(c);
                }
                else
                {
                    Destroy(item);
                }
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
				if(t.gameObject.layer == factory.gameObject.layer)
				{
                    T rt = t.GetComponent<T>();
                    factory.DestoryItem(rt);
				}
			}
		}
	}

	protected void GeneratePlatform ()
	{                                         
		platformFactory.InstantiateItem (_currentPositionY);
	}

	protected void GenerateCoin ()
	{
		int rP = Random.Range (0, 100);
		if (rP <= coinChance) {
			collectibleFactory.InstantiateItem(_currentPositionY + offsetY / 2);
		}
	}

	protected void GenerateEnemy ()
	{
		int rE = Random.Range (0, 100);
		if (rE <= enemyChance) {    
			enemyFactory.InstantiateItem (_currentPositionY + offsetY / 2);
		}
	}
}
