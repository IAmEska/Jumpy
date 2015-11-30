using UnityEngine;
using System.Collections;

public class EnemyFactory : AbstractFactory<Enemy>
{          

    [Range(0,100)]
    public int enemySpawnChance = 5;

    [Range(0,1000)]
    public int spawnDistanceGap = 4;
   
    protected int _enemySpawnPseudoChance;

    [SerializeField]
    protected UnityEngine.UI.Image _fallWarning;

    [SerializeField]
    protected RectTransform _canvas;

    [SerializeField]
    protected int _fallDuration;

    [SerializeField]
    protected float _fallingYOffset = 15f;

    void Start()
    {
        _enemySpawnPseudoChance = enemySpawnChance;                                  
    }

    #region implemented abstract members of AbstractFactory

    public override void InstantiateItem(float positionY)
    {
        if(_lastSpawnPositionY + spawnDistanceGap < positionY)
        { 
            int chance = Random.Range(0, 100);
            if(_enemySpawnPseudoChance >= chance)
            { 
                _positionY = positionY;
                _itemPosition = Random.Range(0, cache.cacheTypes.Length);
                _enemySpawnPseudoChance = enemySpawnChance;
                _lastSpawnPositionY = _positionY;
            }
            else
            {
                _enemySpawnPseudoChance += enemySpawnChance; 
            }
            _state = FactoryState.GENERATE;
        }
    }

    IEnumerator DisableWarningImage()
    {
        yield return new WaitForSeconds(_fallDuration);
        _fallWarning.gameObject.SetActive(false);
    }

    protected override void SpawnItem (Enemy e)
	{
		if (e is ITopSpawn) {
			
            if(e is UpToDownFlyingEnemy)
            {   
                e.transform.position = new Vector3(Random.Range(_minX + e.spriteWidth / 2, _maxX - e.spriteWidth / 2), _positionY + _fallingYOffset);
                StopCoroutine(DisableWarningImage());

                Vector3 canvasPos;
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, e.transform.position);      

                if(RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas, screenPoint, null, out canvasPos))
                {
                    Vector3 pos = _fallWarning.rectTransform.position;
                    pos.x = canvasPos.x;                
                    _fallWarning.gameObject.SetActive(true);
                    _fallWarning.rectTransform.position = pos;
                    
                    StartCoroutine(DisableWarningImage());
                }       
            }
            else
            {
                e.transform.position = new Vector3(Random.Range(_minX + e.spriteWidth / 2, _maxX - e.spriteWidth / 2), _positionY);
            }
			
		} else if (e is IBottomSpawn) {
			
		}
	}

	#endregion
}
