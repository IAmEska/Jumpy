using UnityEngine;
using System.Collections;

public class LineFlyingEnemy : Enemy, ITopSpawn
{
	public float deadForce = 2f;
	public float flySpeed = 15f;
	protected int direction = 1;

	#region implemented abstract members of Enemy
	protected override void StartAdditional ()
	{

	}

	protected override void IdleBehaviour ()
	{

	}
	protected override void AliveBehaviour ()
	{
		var nextPos = transform.position + transform.right * direction * flySpeed * Time.deltaTime;
		if (nextPos.x + spriteWidth / 2 >= _areaMaxX || nextPos.x - spriteWidth / 2 <= _areaMinX) {
			var curScale = transform.localScale;
			curScale.x *= -1;
			transform.localScale = curScale;
			direction *= -1;
		} else {
			transform.position = nextPos;
		}
	}
	protected override void DeadBehaviour ()
	{
		var dir = (Vector2)transform.position - _hitFrom; 
		Debug.Log ("force : " + (dir.normalized * deadForce));
		_rigidbody.velocity = dir.normalized * deadForce;


	}
	#endregion
}
