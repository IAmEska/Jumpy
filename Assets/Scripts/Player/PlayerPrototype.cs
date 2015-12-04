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

	public const string ANIM_FORCE = "Force";

    public GameObject shield;
    public PlayerState state;
    public Rigidbody2D mRigidbody;
    public PlayerBehaviours selectedBehaviour;
    public LayerMask groundMask;
    public float forceY = 400,
        forceX = 30,
        jumpPositionYSpread = 0.1f,
        halfWidth,
        halfHeight,
        safetyNetJump = 50;
    public bool isImmortal = false, isShieldOn = false;

	public AudioClip audioJump;

    public bool isControlSwaped = false;

    protected float _defaultScaleX;
    protected float _rayCastGroundRange = 2f;
    protected float _areaMinX,
        _areaMaxX;

    public int direction = 1;
    protected int _prevDirection = 1;

    protected bool isForceAdded = false,
        _canDoubleJump = true,
        _doDoubleJump = false,
        _animationSet = false,
        _groundedWait = false;

    protected Vector3 _startPosition;
    protected SpriteRenderer _renderer;
    protected Collider2D _collider;
    protected PlayerState _prevState;

    [HideInInspector]
	public Animator _animator;

	protected AudioSource _audioSource;
    protected AbstractPlayerBehaviour _behaviour;
    protected PlayerBehaviours _prevSelectedBehaviour;

    // Use this for initialization
    void Start()
    {
        _startPosition = transform.position;
        _renderer = GetComponent<SpriteRenderer>();
        mRigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator> ();

        halfHeight = _renderer.bounds.size.y / 2;
        halfWidth = _renderer.bounds.size.x / 2;

        _collider = GetComponent<Collider2D>();
		_audioSource = GetComponent<AudioSource> ();

        float sizeX = Camera.main.orthographicSize * Screen.width / Screen.height;
        _areaMinX = Camera.main.transform.position.x - sizeX;
        _areaMaxX = Camera.main.transform.position.x + sizeX;

        _defaultScaleX = transform.localScale.x;
    }

    public void BurstJump(float value)
    {
        isImmortal = true;
        _canDoubleJump = false;
        _animator.SetFloat(ANIM_FORCE, 0.5f);
        state = PlayerState.InAir;
        mRigidbody.velocity = transform.up * value;
    }

    public void SetShield(bool isActive)
    {
        isShieldOn = isActive;
        shield.SetActive(isActive);
    }

  /*  IEnumerator JumpAnimation()
    {
        yield return new WaitForSeconds(0.35f);
        
    }  */

    IEnumerator GroundedWait()
    {
        yield return new WaitForSeconds(0.05f);
        _audioSource.PlayOneShot(audioJump);
        state = PlayerState.Grounded;
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
                if (isImmortal)
                    isImmortal = false;

                // StopCoroutine(JumpAnimation());
                //StartCoroutine(JumpAnimation());
                _groundedWait = false;
                
                _canDoubleJump = true;    
                _behaviour.GroundedBehaviour();
                break;

            case PlayerState.InAir:

                if(_prevState != state)
                    _animator.SetFloat(ANIM_FORCE, 0.5f);

                
                if (mRigidbody.velocity.y <= 0)
                {
                    state = PlayerState.Falling;

                }

                _behaviour.InAirBehaviour();

                if (!_animationSet && mRigidbody.velocity.y <= 0.5f)
                {
                    _animationSet = true;
                    _animator.SetFloat(ANIM_FORCE, 1);
                }

                break;

            case PlayerState.Falling:     
                _animationSet = false;
                _behaviour.FallingBehaivour();
                if(!_groundedWait && CheckGroundCollision())
                {
                    _groundedWait = true;
                    _animator.SetFloat(ANIM_FORCE, 0);
                    StopCoroutine(GroundedWait());
                    StartCoroutine(GroundedWait());
                }
                break;

        }

        if (transform.position.x < _areaMinX)
            transform.position = new Vector3(_areaMaxX, transform.position.y);

        if (transform.position.x > _areaMaxX)
            transform.position = new Vector3(_areaMinX, transform.position.y);

        _prevState = state;
        _prevSelectedBehaviour = selectedBehaviour;
        
        if(direction != _prevDirection)
        { 
            Vector3 newScale = transform.localScale;
            newScale.x = _defaultScaleX * direction;
            transform.localScale = newScale;
        }

        _prevDirection = direction;
    }

    protected bool CheckGroundCollision()
    {
        bool ret = false;
        RaycastHit2D[] hits= Physics2D.CircleCastAll(transform.position, 0.3f, transform.up * -1, _rayCastGroundRange, groundMask.value);
        foreach(RaycastHit2D hit in hits)
		{
			if(hit.transform != null)
	        {
				if(hit.distance <= 1)
				{
					Platform p = hit.transform.GetComponent<Platform>();
                    if(p != null)
                    {       
					    if(transform.position.y - halfHeight + 0.15f >= p.transform.position.y + p.platformHeight/2)
		                {   
                            if(p.CompareTag(Constants.TAG_SAFETY_NET))
                            {                          
                                BurstJump(safetyNetJump);
                                p.gameObject.SetActive(false);
                                return false;
                            }

                            ret = true;
		                    DestroyablePlatform dp = hit.transform.gameObject.GetComponent<DestroyablePlatform>();
		                    if (dp != null)
		                    {
		                        dp.StartFalling();
		                    }
		                }
                    }
                    else
                    {
                        Enemy e = hit.transform.GetComponent<Enemy>();
                        if (e != null && e._hitsLeft > 0)
                        {
                            if (transform.position.y - halfHeight + 0.5f >= e.transform.position.y + e.spriteHeight / 2)
                            {                
                                e.Hit();
                                ret = true;
                            }
                        }
                    }   
                }
			}
        }
        return ret;                          
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
        shield.SetActive(false);
        isShieldOn = false;
        isImmortal = false;
        state = PlayerState.Idle;
	}
}
