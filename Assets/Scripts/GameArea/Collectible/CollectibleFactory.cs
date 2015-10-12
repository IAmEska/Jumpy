using UnityEngine;
using System.Collections;  

public class CollectibleFactory : AbstractFactory<Collectible> {

    #region implemented abstract members of AbstractFactory

    public override void InstantiateItem(float positionY)
    {
        _positionY = positionY;
        _itemPosition = 0;
    }

    protected override void SpawnItem (Collectible c)
	{
        //Collectible c = cache.Get(_itemPosition); //Instantiate<Collectible>(items[_itemPosition]);   
		c.transform.position = new Vector3(Random.Range(_minX, _maxX), _positionY);
	}
  
	#endregion
}
