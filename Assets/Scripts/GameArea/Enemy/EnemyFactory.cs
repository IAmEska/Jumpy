using UnityEngine;
using System.Collections;

public class EnemyFactory : AbstractFactory<Enemy>
{
    public float edgeXOffset = 1.5f;

    #region implemented abstract members of AbstractFactory

    public override void InstantiateItem(float positionY)
    {
        _positionY = positionY;
        _itemPosition = Random.Range(0, cache.cacheTypes.Length);
    }

    protected override void SpawnItem (Enemy e)
	{
		if (e is ITopSpawn) {
			e.transform.position = new Vector3 (Random.Range (_minX + edgeXOffset, _maxX - edgeXOffset), _positionY);
			
		} else if (e is IBottomSpawn) {
			
		}
	}

	#endregion
}
