using UnityEngine;
using System.Collections;


public class UpToDownFlyingEnemy : Enemy, ITopSpawn
{
	public float deadForce = 2f;
	public float flySpeed = 10f;
	protected int direction = 1;

	#region implemented abstract members of Enemy

	protected override void IdleBehaviour ()
	{

	}

	protected override void AliveBehaviour ()
	{
		var nextPos = transform.position + -transform.up * direction * flySpeed * Time.deltaTime;
		transform.position = nextPos;
	}

	protected override void DeadBehaviour ()
	{

	}

	protected override void StartAdditional ()
	{

	}

	#endregion
}
