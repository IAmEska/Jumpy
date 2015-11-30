using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour
{
    public enum PlatformState
    {
        Idle,
        Active,
        Destory
    }

    public PlatformState state, prevState;
	public float maxWidth = 3f, minWidth = 1f;
    public float platformHeight, platformWidth;
    public Sprite[] sprites;
    public float minPosX, maxPosX;
    public float scaleAnimationTime = 1.5f;
    public LevelGenerator levelGenerator;  

    protected Rigidbody2D _rigidbody;
    protected SpriteRenderer _spriteRenderer;
    protected PlatformTypeChance.PlatformType _platformToAdd;
    protected bool _addPlatformComponent = false,
        _runScaleAnimation = false;

    protected float _resizeTo; 

    // Use this for initialization
    void Awake ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer> ();
        _rigidbody = GetComponent<Rigidbody2D>();
        platformHeight = _spriteRenderer.bounds.size.y;
        platformWidth = _spriteRenderer.bounds.size.x;
        _addPlatformComponent = false;
        _runScaleAnimation = false; 
        state = PlatformState.Idle;
        prevState = PlatformState.Idle;
    }

    public void SetSprite(int position)
    {
        if(sprites.Length > position)
            _spriteRenderer.sprite = sprites[position];
    }

    public void SetKinematic(bool enabled)
    {
        _rigidbody.isKinematic = enabled;
    }

    public void AddPlatformComponent(PlatformTypeChance.PlatformType type)
    {
        _platformToAdd = type;
        _addPlatformComponent = true;     
    }

    public void StartResizeAnimation(float toScaleX)
    {                             
        _runScaleAnimation = true;
        _resizeTo = toScaleX;
    }

    public void FixedUpdate()
    {
        if(state == PlatformState.Active)
        { 
            if(_addPlatformComponent)
            {
                if (_platformToAdd == PlatformTypeChance.PlatformType.Destroyable)
                {
                    DestroyablePlatform dp = GetComponent<DestroyablePlatform>();
                    if (dp == null)
                    { 
                        gameObject.AddComponent<DestroyablePlatform>();
                    }
                }
                else if (_platformToAdd == PlatformTypeChance.PlatformType.MovingPlatform)
                {
                    MovingPlatform mp = GetComponent<MovingPlatform>();
                    if (mp == null)
                    { 
                        gameObject.AddComponent<MovingPlatform>();
                    }
                }
            }

            if(_runScaleAnimation)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = _resizeTo;

                transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * scaleAnimationTime);
                platformWidth = newScale.x;
                if(Mathf.Abs(transform.localScale.x - newScale.x) <= 0.2f)
                {
                    _runScaleAnimation = false;
                }
            }

            if(levelGenerator)
            { 
                if (Mathf.Abs(transform.position.y - Camera.main.transform.position.y) >= levelGenerator.platfromDestroyDistance)
                    levelGenerator.DestroyObject(gameObject);
            }
        }
    }

    public void Reset()
    {
        SetKinematic(true);
        _addPlatformComponent = false;
        _runScaleAnimation = false;

        MovingPlatform mp = gameObject.GetComponent<MovingPlatform>();
        if (mp != null)
            Destroy(mp);

        DestroyablePlatform dp = gameObject.GetComponent<DestroyablePlatform>();
        if (dp != null)
            Destroy(dp);          

        
        state = PlatformState.Idle;
        prevState = PlatformState.Idle;
        
    }
                                                                 
}
