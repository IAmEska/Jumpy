﻿using UnityEngine;
using System.Collections;

public class PlatformFactory : AbstractFactory<Platform> {

	public StartPlatform startPlatform;
    public float offsetX = 2f,               
        minPlatformWidth = 0.5f,
        maxPlatformWidth = 1.5f;
                                       
    public int maxPlatformAtOnce = 2,
        minPlatformAtOnce = 1;

    public bool isAdvancedPlatforms = false;

    public PlatformTypeChance[] components;
                                                        
	protected Platform _prevPlatform;       

    void Start()
    {
        foreach(PlatformTypeChance p in components)
        {
            p.pseudoChance = p.chance;
            p.defualtChance = p.chance;
            p.defaultMinimumSameComponentGap = p.minimumSameComponentGap;
        }
    }

    public void Reset()
    {
        foreach (PlatformTypeChance p in components)
        {
            p.chance = p.defualtChance;
            p.pseudoChance = p.defualtChance;   
            p.minimumSameComponentGap = p.defaultMinimumSameComponentGap;
            p.lastSpawnPositionY = 0;
        }
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

    public override void DestoryItem(Component item)
    {
        foreach (PlatformTypeChance ptc in components)
        {
            Component c = item.GetComponent(ptc.GetComponent());
            if (c != null)
                Destroy(c);
        }
        base.DestoryItem(item);
    }

    #region implemented abstract members of AbstractFactory
    protected override void SpawnItem (Platform p)
	{

        p.SetSprite(0);
        if (isAdvancedPlatforms)
        { 
            foreach(PlatformTypeChance ptc in components)
            {
                if(ptc.lastSpawnPositionY + ptc.minimumSameComponentGap < _positionY)
                { 
                    float chance = Random.Range(0, 100);
                    if (ptc.pseudoChance >=  chance)
                    {
                        ptc.pseudoChance = ptc.chance;
                        ptc.lastSpawnPositionY = _positionY;
                        p.gameObject.AddComponent(ptc.GetComponent());
                    }
                    else
                    {
                        ptc.pseudoChance += chance;
                    }
                }
            }              
        }

        float posX = Random.Range (_minX, _maxX);
		if (_prevPlatform != null) {
			if (Mathf.Abs (posX - _prevPlatform.transform.position.x) < offsetX) {
				int r = Random.Range (0, 2);
				if (r == 0) {
					posX -= offsetX;
					if (posX < _minX)
						posX = _maxX - Mathf.Abs (posX);
				} else {
					posX += offsetX;
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