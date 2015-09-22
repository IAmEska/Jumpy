using UnityEngine;
using System.Collections;

public class CollectibleFactory : AbstractFactory<Collectible> {

	#region implemented abstract members of AbstractFactory

	protected override void SpawnItem ()
	{
		Collectible c = Instantiate<Collectible>(items[_itemPosition]);
		c.transform.parent = transform;
		c.transform.position = new Vector3(Random.Range(_minX, _maxX), _positionY);
	}

	public override void DestoryItem (Collectible item)
	{

	}
	#endregion
}
