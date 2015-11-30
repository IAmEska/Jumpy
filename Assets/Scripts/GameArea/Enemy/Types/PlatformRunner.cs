using UnityEngine;
using System.Collections;

public class PlatformRunner : Enemy
{

	public float speed = 2f, speedY = 4f;
	public float landingDistance = 0.15f;
	public LayerMask groundMask;

	protected int direction = 1;


	#region implemented abstract members of Enemy
	protected override void IdleBehaviour ()
	{

	}
	protected override void AliveBehaviour ()
	{
		RaycastHit2D currentHit = Physics2D.Raycast (transform.position, -transform.up, spriteHeight / 2 + landingDistance, groundMask);
		if (currentHit.transform == null) {
			transform.position = transform.position + -transform.up * direction * speedY * Time.deltaTime;
		} else {
			Vector3 nextPosition = transform.position + transform.right * direction * speed * Time.deltaTime;
			RaycastHit2D hit = Physics2D.Raycast (nextPosition, -transform.up, spriteHeight / 2 + landingDistance, groundMask);
			if (hit.transform == null) {
				var curScale = transform.localScale;
				curScale.x *= -1;
				transform.localScale = curScale;
				direction *= -1;
			} else {
				transform.position = nextPosition;
			}
		}
	}
	protected override void DeadBehaviour ()
	{

	}
	protected override void StartAdditional ()
	{

	}
	#endregion
}
