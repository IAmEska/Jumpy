using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Enemy : MonoBehaviour
{

	public enum Status
	{
		Idle,
		Alive,
		Dead
	}

	public int hitsToDie = 1;
    public Status status;

	protected Rigidbody2D _rigidbody;
	protected Collider2D _collider;
	protected SpriteRenderer _renderer;

	protected Vector2 _hitFrom;
	public int _hitsLeft;
	protected Status  _prevStatus;

	protected float _areaMinX, _areaMaxX;
	public float spriteWidth, spriteHeight;

	// Use this for initialization
	void Awake ()
	{
		_rigidbody = GetComponent<Rigidbody2D> ();
		_collider = GetComponent<Collider2D> ();
		_renderer = GetComponent<SpriteRenderer> ();

		spriteWidth = _renderer.bounds.size.x;
		spriteHeight = _renderer.bounds.size.y;
		float sizeX = Camera.main.orthographicSize * Screen.width / Screen.height;
		_areaMinX = Camera.main.transform.position.x - sizeX;
		_areaMaxX = Camera.main.transform.position.x + sizeX;
		Reset ();
		StartAdditional ();
	}

	void Reset ()
	{
        status = Status.Alive;
		_hitsLeft = hitsToDie;
		_rigidbody.velocity = Vector2.zero;
		_rigidbody.isKinematic = true;
		_collider.isTrigger = true;
		_collider.enabled = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (_prevStatus != status) {
			switch (status) {
			case Status.Dead:
				DeadBehaviour ();
				break;
			case Status.Idle:
				IdleBehaviour ();
				break;
			}
		}

		if (status == Status.Alive)
			AliveBehaviour ();

		_prevStatus = status;
	}

	protected abstract void IdleBehaviour ();
	protected abstract void AliveBehaviour ();
	protected abstract void DeadBehaviour ();
	protected abstract void StartAdditional ();

	public virtual void Hit ()
	{
		if (_hitsLeft > 0 && status != Status.Dead) {
			_hitsLeft = Mathf.Max (0, _hitsLeft - 1);
			if (_hitsLeft == 0) {
                if(_rigidbody)
				    _rigidbody.isKinematic = false;

                if (_collider)
                    _collider.enabled = false;

                
                var pos = transform.position;
				pos.y -= 1;
				_hitFrom = pos;
                status = Status.Dead;     
            }
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (status == Status.Alive) {
			if (other.CompareTag(Constants.TAG_PLAYER)) {
                if (_hitsLeft > 0)
                { 
                    PlayerPrototype pp = other.GetComponent<PlayerPrototype>();
                    if(pp.isImmortal || pp.isShieldOn)
                    {
					    if(pp.isShieldOn && !pp.isImmortal)
                    	    pp.SetShield(false);

                    
                            Hit();
                    }
                    else
                    { 
                        pp.Kill ();
                    }
                }
            }
		}
	}


}
