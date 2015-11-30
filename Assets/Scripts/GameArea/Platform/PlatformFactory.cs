using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformFactory : AbstractFactory<Platform> {

	public StartPlatform startPlatform;
    public float offsetX = 2f,               
        minPlatformWidth = 0.5f,
        maxPlatformWidth = 2.5f,
        defaultMinPlatformWidth,
        defaultMaxPlatformWidth;
                                       
    public int maxPlatformAtOnce = 2,
        minPlatformAtOnce = 1;

    public int multiplePlatformChance = 25;
    public int missingPlatformChance = 10;
    public int minimumMissingPlatformGap = 2;

    protected float _lastMissingPlatformPosY = 0;

    public bool isAdvancedPlatforms = false;

    public PlatformTypeChance[] components;
                                                        
	protected Platform _prevPlatform;
    protected List<int> familyUsed;      

    void Start()
    {
        familyUsed = new List<int>();
        foreach (PlatformTypeChance p in components)
        {
            p.pseudoChance = p.chance;
            p.defualtChance = p.chance;
            p.defaultMinimumSameComponentGap = p.minimumSameComponentGap;
        }
        defaultMaxPlatformWidth = maxPlatformWidth;
        defaultMinPlatformWidth = minPlatformWidth; 
    }

    protected override void ResetBehaviour()
    {
        foreach (PlatformTypeChance p in components)
        {
            p.chance = p.defualtChance;
            p.pseudoChance = p.defualtChance;
            p.minimumSameComponentGap = p.defaultMinimumSameComponentGap;
            p.lastSpawnPositionY = 0;
        }

        maxPlatformWidth = defaultMaxPlatformWidth;
        minPlatformWidth = defaultMinPlatformWidth;
    }

    protected override void GenerateBehaviour()
    {                    
            _prevPlatform = null;

            int missingChance = Random.Range(0, 100);
            if(missingPlatformChance >= missingChance && _positionY - _lastMissingPlatformPosY >= minimumMissingPlatformGap)
            {
                _lastMissingPlatformPosY = _positionY;
                return;
            }

            int platformCount = minPlatformAtOnce;
            int multipleChance = Random.Range(0, 100);     
            if (multiplePlatformChance >= multipleChance)
            { 
                platformCount = Random.Range(minPlatformAtOnce + 1, maxPlatformAtOnce + 1);
            }

            if (_itemPosition >= 0 && _itemPosition < cache.cacheTypes.Length)
            {
                for(int i =0; i<platformCount; i++)
                { 
                    Platform item = cache.Get(_itemPosition);   
                    item.gameObject.SetActive(true);       
                    item.transform.SetParent(transform);   
                    SpawnItem(item);
                }
            }
                                   
        _itemPosition = -1;
    }

    public StartPlatform InstantiateStartPlatform(float starPosY)
	{
        StartPlatform sp = Instantiate(startPlatform);
        sp.transform.position = new Vector3(0, starPosY, 0);
        sp.transform.SetParent(transform);
        return sp;
	}

    public override void SetBounds(float minX, float maxX)
    {
        base.SetBounds(minX, maxX);
        _maxX = maxX;
        _minX = minX;
    }

    public override void InstantiateItem(float positionY)
    {
        _state = FactoryState.GENERATE;
        _itemPosition = 0;
        _positionY = positionY;
    }

    public override void DestoryItem(Component item)
    {     
        Component[] components = item.GetComponents<PlatformComponent>();
        for(int i=0; i<components.Length; i++)
        {
            Destroy(components[i]);
        }
         
        base.DestoryItem(item);
    }

    #region implemented abstract members of AbstractFactory
    protected override void SpawnItem (Platform p)
	{                    
        p.SetSprite(0);
        p.minPosX = _minX;
        p.maxPosX = _maxX;              
        
        if (isAdvancedPlatforms)
        {                     
            foreach(PlatformTypeChance ptc in components)
            {
                if(ptc.lastSpawnPositionY + ptc.minimumSameComponentGap < _positionY)
                {
                    if (!familyUsed.Contains(ptc.family))
                    {
                        float chance = Random.Range(0, 100);
                        if (ptc.pseudoChance >= chance)
                        {
                            ptc.pseudoChance = ptc.chance;
                            ptc.lastSpawnPositionY = _positionY;
                            p.gameObject.AddComponent(ptc.GetComponent());
                        }
                        else
                        {
                            ptc.pseudoChance += chance;
                        }
                        familyUsed.Add(ptc.family);
                    }
                }
            }
            familyUsed.Clear();              
        }

        float posX;
		if (_prevPlatform != null) {
            int direction = Random.Range(0, 2);

            float remainingLeftDistance = _minX - (_prevPlatform.transform.position.x - _prevPlatform.platformWidth/2 - offsetX);
            float remainingRightDistance = _maxX - (_prevPlatform.transform.position.x + _prevPlatform.platformWidth/2 + offsetX);

            if(direction == 0) //check if we can move platform to left
            {
                if (remainingLeftDistance > 0 || Mathf.Abs(remainingLeftDistance) < minPlatformWidth) // we need switch direction, there is no space in left
                    direction = 1;
            }
            else
            {
                if (remainingRightDistance < 0 || Mathf.Abs(remainingRightDistance) < minPlatformWidth) // we need switch direction, there is no space in right
                    direction = 0;
            }

            float maxWidth = maxPlatformWidth;
            float abs = direction == 0 ? Mathf.Abs(remainingLeftDistance) : Mathf.Abs(remainingRightDistance);
            if (abs < maxPlatformWidth)
                maxWidth = abs;

            float newWidth = Random.Range(minPlatformWidth, maxWidth);
            p.platformWidth = newWidth;
            p.transform.localScale = new Vector3(newWidth, p.transform.localScale.y, 1);
                                     
            // calculate remaining position
            if(direction == 0)
            {
                float remaining = Mathf.Abs(remainingLeftDistance) - newWidth;
                posX = newWidth/2 + Random.Range(_minX, _minX + remaining);
            }
            else
            {
                float remaining = (Mathf.Abs(remainingRightDistance) - newWidth);
                posX = Random.Range(_maxX - remaining, _maxX) - newWidth/2;
            }         
		}
        else
        {
            float newWidth = Random.Range(minPlatformWidth, maxPlatformWidth);
            p.platformWidth = newWidth;
            posX = Random.Range(_minX+newWidth/2, _maxX-newWidth/2);
            p.transform.localScale = new Vector3(newWidth, p.transform.localScale.y, 1);
        }
		
		p.transform.position = new Vector3 (posX, _positionY);      
        p.state = Platform.PlatformState.Active;
        _prevPlatform = p;
	}

	#endregion
}