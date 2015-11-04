using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Platform : MonoBehaviour
{
	public float maxWidth = 3f, minWidth = 1f;
    public float platformHeight, platformWidth;
    public Sprite[] sprites;
    
    protected Rigidbody2D _rigidbody;
    protected SpriteRenderer _spriteRenderer;

    // Use this for initialization
    void Awake ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer> ();
        _rigidbody = GetComponent<Rigidbody2D>();
        platformHeight = _spriteRenderer.bounds.size.y;
        platformWidth = _spriteRenderer.bounds.size.x;
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

    public void Reset()
    {
        MovingPlatform mp = gameObject.GetComponent<MovingPlatform>();
        if (mp != null)
            Destroy(mp);

        DestroyablePlatform dp = gameObject.GetComponent<DestroyablePlatform>();
        if (dp != null)
            Destroy(dp);

        var collider = GetComponent<BoxCollider2D>();
        collider.offset = new Vector2(0, 0);
        collider.size = new Vector2(1, 1);
        _rigidbody.velocity = Vector2.zero;
        SetKinematic(true);
    }
                                                                 
}
