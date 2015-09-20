using UnityEngine;
using System.Collections;
using System;

public class EnemyFactory : MonoBehaviour
{

	public Enemy[] enemies;
	protected float _positionY;
	protected Vector2 _boundsX;
	protected int _enemyType;

	void Start ()
	{
		_enemyType = -1;
	}

	void FixedUpdate ()
	{
		if (_enemyType >= 0) {

			Enemy e = Instantiate<Enemy> (enemies [_enemyType]);
			if (e is ITopSpawn) {
				e.transform.position = new Vector3 (UnityEngine.Random.Range (_boundsX.x, _boundsX.y), _positionY);
				e.transform.parent = transform;
			} else if (e is IBottomSpawn) {

			}


			_enemyType = -1;
		}
	}

	public void InstantiateEnemy (int type, float positionY, Vector2 boundsX)
	{
		_enemyType = type;
		_positionY = positionY;
		_boundsX = boundsX;
	}

}
