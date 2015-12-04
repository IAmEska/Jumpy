using UnityEngine;
using System.Collections;

public class LineyFlyingDropEnemy : LineFlyingEnemy
{

	public float dropItemMinTime = 2f, dropItemMaxTime = 4f;
	public Enemy dropItem;

	protected float _dropTime;
	protected float _cratedTime;
	protected bool _isDropped;

	protected override void StartAdditional ()
	{
		base.StartAdditional ();
		_cratedTime = Time.timeSinceLevelLoad;
		_dropTime = _cratedTime + Random.Range (dropItemMinTime, dropItemMaxTime);
		_isDropped = false;
	}

	protected override void AliveBehaviour ()
	{
		base.AliveBehaviour ();
		if (!_isDropped && dropItem) {
			if (Time.timeSinceLevelLoad >= _dropTime) {
				_isDropped = true;
				dropItem.transform.parent = transform.parent;
				dropItem.GetComponent<Rigidbody2D> ().isKinematic = false;
			}
		}
	}

    public override void Hit()
    {
        base.Hit();
        if(dropItem)
            dropItem.Hit();
    }
}
