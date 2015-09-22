using UnityEngine;
using System.Collections;
using System;

public class EnemyFactory : AbstractFactory<Enemy>
{
	#region implemented abstract members of AbstractFactory
	protected override void SpawnItem ()
	{
		Enemy e = Instantiate<Enemy> (items [_itemPosition]);
		if (e is ITopSpawn) {
			e.transform.position = new Vector3 (UnityEngine.Random.Range (_minX, _maxX), _positionY);
			e.transform.parent = transform;
		} else if (e is IBottomSpawn) {
			
		}
	}

	public override void DestoryItem (Enemy item)
	{

	}
	#endregion
}
