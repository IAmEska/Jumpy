﻿using UnityEngine;
using System.Collections;

public class PlayerPrototype : MonoBehaviour
{
	public enum PlayerState
	{
		Idle,
		Grounded,
		InAir,
		Falling,
		Dead
	}

	public enum PlayerBehaviours
	{
		JumpingBehaviour
	}

    public PlayerState state;
    public Rigidbody2D mRigidbody;
	public PlayerBehaviours selectedBehaviour;
	public LayerMask groundMask;
	public float forceY = 400, 
		forceX = 30, 
		jumpPositionYSpread = 0.1f, 
		fallingThreshold = 0.5f,
		groundedTreshold = 0.2f,
		groundedNegativeTreshold = -0.15f;

	protected float _halfHeight, 
		_halfWidth, 
		_areaMinX, 
		_areaMaxX;

	protected bool isForceAdded = false;
	protected Vector3 _startPosition;
	protected SpriteRenderer _renderer;    
    protected PlayerState	_prevState;

	protected AbstractPlayerBehaviour _behaviour;
	protected PlayerBehaviours _prevSelectedBehaviour;

	// Use this for initialization
	void Start ()
	{
		_startPosition = transform.position;
		_renderer = GetComponent<SpriteRenderer> ();
		mRigidbody = GetComponent<Rigidbody2D> ();
		_halfHeight = _renderer.bounds.size.y / 2;
		_halfWidth = _renderer.bounds.size.x / 2;

		float sizeX = Camera.main.orthographicSize * Screen.width / Screen.height;
		_areaMinX = Camera.main.transform.position.x - sizeX;
		_areaMaxX = Camera.main.transform.position.x + sizeX;
	}

	void FixedUpdate ()
	{
		if (_prevSelectedBehaviour != selectedBehaviour || _behaviour == null) {
			if (_behaviour != null)
				Destroy (_behaviour);

			System.Type type;
			switch (selectedBehaviour) {
			default:
				type = typeof(JumpingPlayerBehaviour);
				break;
			}

			_behaviour = gameObject.AddComponent (type) as AbstractPlayerBehaviour;
		}

		switch (state) {
		case PlayerState.Idle:
			//Do Nothing
			break;

            case PlayerState.Dead:
                if(_prevState != state)
                {
                    transform.Rotate(Vector3.forward, 180);
                }
                break;

		case PlayerState.Grounded:
			_behaviour.GroundedBehaviour ();
			break;

		case PlayerState.InAir:
			_behaviour.InAirBehaviour ();
			if (mRigidbody.velocity.y <= fallingThreshold) {
				state = PlayerState.Falling;
			}
			break;

		case PlayerState.Falling:
			_behaviour.FallingBehaivour ();
			var actualPosition = transform.position;
			actualPosition.y -= _halfHeight - jumpPositionYSpread;

			var hit = Physics2D.CircleCast (actualPosition, _halfWidth, -transform.up, 1, groundMask);
			if (hit.transform != null) {
				Vector3 dist = actualPosition - hit.transform.position;
				
				var hitRenderer = hit.transform.GetComponent<SpriteRenderer> ();
				if (hitRenderer != null) {
					float hY = hitRenderer.bounds.size.y / 2;
					dist.y -= hY;
					
					if (dist.y <= groundedTreshold && dist.y >= groundedNegativeTreshold) {
						state = PlayerState.Grounded;
					}
				}
			}
			break;

		}

		if (transform.position.x < _areaMinX)
			transform.position = new Vector3 (_areaMaxX, transform.position.y);

		if (transform.position.x > _areaMaxX)
			transform.position = new Vector3 (_areaMinX, transform.position.y);

		_prevState = state;
		_prevSelectedBehaviour = selectedBehaviour;

	}



	public void SetState (PlayerState state)
	{
		this.state = state;
	}

	public void Kill ()
	{
		state = PlayerState.Dead;
	}

	public void Reset ()
	{
        transform.rotation = Quaternion.identity;
        transform.position = _startPosition;
		if (mRigidbody != null)
			mRigidbody.velocity = Vector2.zero;

                 
		state = PlayerState.Idle;
	}
}
