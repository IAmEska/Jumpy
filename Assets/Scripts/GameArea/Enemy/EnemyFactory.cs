using UnityEngine;
using System.Collections;
using System;

public class EnemyFactory : AbstractFactory<Enemy>
{


	#region implemented abstract members of AbstractFactory
	protected override void SpawnItem (Enemy e)
	{
        //Enemy e = cache.Get(_itemPosition); //Instantiate<Enemy> (items [_itemPosition]);
		if (e is ITopSpawn) {
			e.transform.position = new Vector3 (UnityEngine.Random.Range (_minX, _maxX), _positionY);
			
		} else if (e is IBottomSpawn) {
			
		}
	}

	#endregion
}
