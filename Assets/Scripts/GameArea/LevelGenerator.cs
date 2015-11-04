using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{

	//public EnemyFactory enemyFactory;
	//public CollectibleFactory collectibleFactory;

    public FactoryTypeInfo[] factories;

	public bool generate = false;

	public float offsetY;                  

	protected float _startPosY, _currentPositionY;
	protected float _areaMinX, _areaMaxX;
	protected StartPlatform _startPlatformRef;
    protected PlatformFactory _platformFactory;

    public bool isAllCleared = false;

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

        foreach(FactoryTypeInfo fti in factories)
        {
            fti.factory.SetBounds(_areaMinX, _areaMaxX);
            if(fti.factory is PlatformFactory)
            {
                _platformFactory = fti.factory as PlatformFactory;
            }
        }
	}

	public void Reset ()
	{
        _platformFactory.Reset();
		_currentPositionY = _startPosY + offsetY;
        Debug.Log("offset:" + _currentPositionY);
        generate = false;
        if (!_isFirstRun)
        { 
		    _initStartPlatform = true;
            isAllCleared = false;
            ClearAll();
        }
        else
        {
            isAllCleared = true;
            _isFirstRun = false;
        }
              
        
	}

	void FixedUpdate ()
	{
		if (_initStartPlatform) {
            _startPlatformRef = _platformFactory.InstantiateStartPlatform(_startPosY);
			_initStartPlatform = false;
		}

		if (generate) {
            if (_currentPositionY < Camera.main.transform.position.y + Camera.main.orthographicSize)
            {
                _platformFactory.isAdvancedPlatforms = true;

                foreach (FactoryTypeInfo fti in factories)
                {
                    float position = _currentPositionY;
                    if (fti.addOffset)
                        position += offsetY / 2;

                    if(fti.generateFromStart)
                    {
                        fti.factory.InstantiateItem(position);
                    }
                    else if(_currentPositionY > _startPosY + 2 * Camera.main.orthographicSize)
                    {
                        fti.factory.InstantiateItem(position);
                    }
                }
                _currentPositionY += 2;
            }
		}
	}

    public void SetDeadStatus()
    {                           
        if(_startPlatformRef != null)
            _startPlatformRef.GetComponent<Collider2D>().isTrigger = true;

        generate = false;
        _currentPositionY = _startPosY + offsetY;
        _platformFactory.isAdvancedPlatforms = false;
    }

    public void DestroyObject(GameObject item)
    {
        bool destroyed = false;
        foreach(FactoryTypeInfo fti in factories)
        {
            Component c = item.GetComponent(fti.factory.ItemType());
            if(c != null)
            {
                fti.factory.DestoryItem(c);
                destroyed = true;
                break;
            }
        }

        if(!destroyed)
            Destroy(item);      
    }

	protected void ClearAll ()
	{
        foreach(FactoryTypeInfo fti in factories)
        {
            Clear(fti.factory);
        }
        isAllCleared = true;
    }

	protected void Clear (Factory factory)
	{
		if (factory.transform.childCount > 0) {
			for (int i=0; i< factory.transform.childCount; i++) {
				var t = factory.transform.GetChild (i);
				if(t.gameObject.layer == factory.gameObject.layer)
				{
                    Component rt = t.GetComponent(factory.ItemType());
                    factory.DestoryItem(rt);
				}
			}
		}
	}        
}
