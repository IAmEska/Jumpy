using UnityEngine;
using System.Collections;

public class CollectibleFactory : AbstractFactory<Collectible> {

    [Range(0,100)]
    public int spawnChance = 5;

    protected int _spawnPseudoChance;

    public CollectibleManager collectibleManager;

    void Start()
    {
        _spawnPseudoChance = spawnChance;
    }

    #region implemented abstract members of AbstractFactory

    public override void InstantiateItem(float positionY)
    {                                  
        int chance = Random.Range(0, 100);
        if(_spawnPseudoChance >= chance)
        { 
            _positionY = positionY;
            _itemPosition = Random.Range(0,cache.cacheTypes.Length);
            _spawnPseudoChance = 0;
        }
        else
        {
            _spawnPseudoChance += spawnChance;
        }
        _state = FactoryState.GENERATE;
    }

    protected override void SpawnItem (Collectible c)
	{
        //Collectible c = cache.Get(_itemPosition); //Instantiate<Collectible>(items[_itemPosition]);  
        c.collectibleManager = collectibleManager; 
		c.transform.position = new Vector3(Random.Range(_minX + c.spriteWidth/2, _maxX - c.spriteWidth/2), _positionY);
	}
  
	#endregion
}
