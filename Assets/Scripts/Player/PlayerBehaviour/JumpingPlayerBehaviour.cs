using UnityEngine;
using System.Collections;

public class JumpingPlayerBehaviour : AbstractPlayerBehaviour
{

    #region implemented abstract members of AbstractPlayerBehaviour


	public override void GroundedBehaviour ()
	{
        _playerPrototype.mRigidbody.velocity = transform.up * _playerPrototype.forceY;
        LeftRightMove ();
		_playerPrototype.SetState (PlayerPrototype.PlayerState.InAir);
	}

	public override void InAirBehaviour ()
	{
        CheckDoubleJump();
		LeftRightMove ();
	}

	public override void FallingBehaivour ()
	{
        CheckDoubleJump();
        LeftRightMove ();
	}

    protected void LeftRightMove()
    {
        float x = Input.acceleration.x * (_playerPrototype.isControlSwaped ? -1 : 1); //Input.GetAxis ("Horizontal");
        _lowPassValue = Mathf.Lerp(_lowPassValue, x, _lowPassFilterFactor);
		float currentY = _playerPrototype.mRigidbody.velocity.y;

        _playerPrototype.mRigidbody.velocity = Vector2.right * _playerPrototype.forceX * GameSettings.sensitivity * _lowPassValue + new Vector2 (0, currentY);

        if (_playerPrototype.mRigidbody.velocity.x < 0)
        {
            _playerPrototype.direction = 1;
        }
        else if (_playerPrototype.mRigidbody.velocity.x > 0)
        { 
            _playerPrototype.direction = -1;
        }
    }

	#endregion


    protected void CheckDoubleJump()
    {
        if(doDoubleJump)
        {
            doDoubleJump = false;
            _playerPrototype.state = PlayerPrototype.PlayerState.InAir;
            _playerPrototype._animator.SetFloat(PlayerPrototype.ANIM_FORCE, 0.5f);
            _playerPrototype.mRigidbody.velocity = transform.up * _playerPrototype.forceY;
        }
    }

}
