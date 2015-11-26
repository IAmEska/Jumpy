using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
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
        halfWidth,
        halfHeight;

	public AudioClip audioJump;

    public bool isControlSwaped = false;

    protected float _rayCastGroundRange = 2f;
    protected float _areaMinX,
        _areaMaxX;



    protected bool isForceAdded = false,
        _canDoubleJump = true,
        _doDoubleJump = false;
    protected Vector3 _startPosition;
    protected SpriteRenderer _renderer;
    protected Collider2D _collider;
    protected PlayerState _prevState;

	protected AudioSource _audioSource;
    protected AbstractPlayerBehaviour _behaviour;
    protected PlayerBehaviours _prevSelectedBehaviour;

    // Use this for initialization
    void Start()
    {
        _startPosition = transform.position;
        _renderer = GetComponent<SpriteRenderer>();
        mRigidbody = GetComponent<Rigidbody2D>();
        halfHeight = _renderer.bounds.size.y / 2;
        halfWidth = _renderer.bounds.size.x / 2;

        _collider = GetComponent<Collider2D>();
		_audioSource = GetComponent<AudioSource> ();

        float sizeX = Camera.main.orthographicSize * Screen.width / Screen.height;
        _areaMinX = Camera.main.transform.position.x - sizeX;
        _areaMaxX = Camera.main.transform.position.x + sizeX;
    }

    void FixedUpdate()
    {
        if (_prevSelectedBehaviour != selectedBehaviour || _behaviour == null) {
            if (_behaviour != null)
                Destroy(_behaviour);

            System.Type type;
            switch (selectedBehaviour) {
                default:
                    type = typeof(JumpingPlayerBehaviour);
                    break;
            }

            _behaviour = gameObject.AddComponent(type) as AbstractPlayerBehaviour;
        }

        if (_doDoubleJump)
        {
            _doDoubleJump = false;
            _behaviour.doDoubleJump = true;
        }

        switch (state) {
            case PlayerState.Idle:
                //Do Nothing
                break;

            case PlayerState.Dead:
                if (_prevState != state)
                {
                    transform.Rotate(Vector3.forward, 180);
                }
                break;

            case PlayerState.Grounded:
                _canDoubleJump = true;
                _behaviour.GroundedBehaviour();
                break;

            case PlayerState.InAir:
                _behaviour.InAirBehaviour();
                if (mRigidbody.velocity.y <= 0)
                {
                    state = PlayerState.Falling;
                }
                break;

            case PlayerState.Falling:
                _behaviour.FallingBehaivour();
                CheckGroundCollision();
                break;

        }

        if (transform.position.x < _areaMinX)
            transform.position = new Vector3(_areaMaxX, transform.position.y);

        if (transform.position.x > _areaMaxX)
            transform.position = new Vector3(_areaMinX, transform.position.y);

        _prevState = state;
        _prevSelectedBehaviour = selectedBehaviour;

    }

    protected void CheckGroundCollision()
    {
        RaycastHit2D[] hits= Physics2D.CircleCastAll(transform.position, 0.2f, transform.up * -1, _rayCastGroundRange, groundMask.value);
        foreach(RaycastHit2D hit in hits)
		{
			if(hit.transform != null)
	        {
				if(hit.distance <= 1)
				{
					Platform p = hit.transform.GetComponent<Platform>();
					if(transform.position.y - halfHeight + 0.15f >= p.transform.position.y + p.platformHeight/2)
		            {                 
						_audioSource.PlayOneShot(audioJump);
		                state = PlayerState.Grounded;
		                DestroyablePlatform dp = hit.transform.gameObject.GetComponent<DestroyablePlatform>();
		                if (dp != null)
		                {
		                    dp.StartFalling();
		                }
		            }    
				}
			}
        }
    }

    public void DoubleJump()
    {
         if(_canDoubleJump)
        {
            _doDoubleJump = true;
            _canDoubleJump = state == PlayerState.Grounded;
        }
    }                                   

    public void SetState (PlayerState state)
	{
		this.state = state;
	}

	public void Kill ()
	{
		state = PlayerState.Dead;
        _collider.isTrigger = true;
	}

	public void Reset ()
	{
        transform.rotation = Quaternion.identity;
        transform.position = _startPosition;
		if (mRigidbody != null)
			mRigidbody.velocity = Vector2.zero;

        _canDoubleJump = true;
        _doDoubleJump = false;
        _collider.isTrigger = false;
        state = PlayerState.Idle;
	}
}
