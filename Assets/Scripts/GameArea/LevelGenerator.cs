using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{

    public enum GeneratorState
    {       
            RESET,
            START,
            GENERATE,
            DEAD,
            IDLE
    }

	//public EnemyFactory enemyFactory;
	//public CollectibleFactory collectibleFactory;

    public FactoryTypeInfo[] factories;

	public bool generate = false;
    public float platfromDestroyDistance = 20f;
	public float offsetY;                  

	protected float _startPosY, _currentPositionY;
	protected float _areaMinX, _areaMaxX;
	protected StartPlatform _startPlatformRef;
    protected PlatformFactory _platformFactory;

    protected GeneratorState _state, _prevState;

	IEnumerator GenerateCoroutine(float positionY)
	{
		foreach (FactoryTypeInfo fti in factories)
		{
			float position = positionY;
			if (fti.addOffset)
				position += offsetY / 2;
			
			if (fti.generateFromStart)
			{
				fti.factory.InstantiateItem(position);
			}
			else if (_currentPositionY > _startPosY + 2 * Camera.main.orthographicSize)
			{
				fti.factory.InstantiateItem(position);
			}
			yield return null;
		}
	}

	// Use this for initialization
	void Start ()
	{                           
        _startPosY = Camera.main.transform.position.y - Camera.main.orthographicSize;         

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

        _state = GeneratorState.RESET;
        _prevState = GeneratorState.IDLE;
    }

    protected void ResetBehaviour ()
	{
		StopAllCoroutines ();
        ClearAll();                                           
		_currentPositionY = _startPosY + offsetY;   
	}

    protected void GenerateBehaviour()
    {
        if (_currentPositionY < Camera.main.transform.position.y + Camera.main.orthographicSize)
        {
            _platformFactory.isAdvancedPlatforms = true;
			StartCoroutine(GenerateCoroutine(_currentPositionY));
            _currentPositionY += 2f;
        }
    }

    protected void DeadBehaviour()
    {
        if (_startPlatformRef != null)
            _startPlatformRef.GetComponent<Collider2D>().isTrigger = true;

        _currentPositionY = _startPosY + offsetY;
        _platformFactory.isAdvancedPlatforms = false;
    }

	void FixedUpdate ()
	{
		switch(_state)
        {
            case GeneratorState.RESET:
                if(_prevState != _state)
                {
                    ResetBehaviour();
                }
                break;

            case GeneratorState.START:
                if(_prevState != _state)
                {
                    _startPlatformRef = _platformFactory.InstantiateStartPlatform(_startPosY);
                    _state = GeneratorState.GENERATE;
                }
                break;

            case GeneratorState.GENERATE:
                GenerateBehaviour();
                break;

            case GeneratorState.DEAD:
                if(_prevState != _state)
                {
                    DeadBehaviour();
                }
                break;
        }
        _prevState = _state;     
	}

    public void SetState(GeneratorState state)
    {
        _state = state;
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
            fti.factory.SetState(Factory.FactoryState.CLEAR);
        }                    
    }      
}
