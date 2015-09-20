using UnityEngine;
using System.Collections;

public class JumpingPlayerBehaviour : AbstractPlayerBehaviour
{


	#region implemented abstract members of AbstractPlayerBehaviour

	public override void GroundedBehaviour ()
	{
		_playerPrototype.rigidbody.velocity = transform.up * _playerPrototype.forceY;
		LeftRightMove ();
		_playerPrototype.SetState (PlayerPrototype.PlayerState.InAir);
	}

	public override void InAirBehaviour ()
	{
		LeftRightMove ();
	}

	public override void FallingBehaivour ()
	{
		LeftRightMove ();
	}

	protected void LeftRightMove ()
	{
		float x = Input.acceleration.x; //Input.GetAxis ("Horizontal");
		_lowPassValue = Mathf.Lerp (_lowPassValue, x, _lowPassFilterFactor);
		float currentY = _playerPrototype.rigidbody.velocity.y;
		_playerPrototype.rigidbody.velocity = Vector2.right * _playerPrototype.forceX * _lowPassValue + new Vector2 (0, currentY);
	}

	#endregion


}
