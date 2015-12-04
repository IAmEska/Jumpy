using UnityEngine;
using System.Collections;

public abstract class Collectible : MonoBehaviour
{                       
    public AudioManager audioManager;

    public CollectibleManager collectibleManager;
    public float spriteWidth;
    public float spriteHeight;

    public AudioManager.AudioSound sound;

    protected SpriteRenderer _renderer;

	void Start ()
	{
        _renderer = GetComponent<SpriteRenderer>();
        spriteWidth = _renderer.bounds.size.x;
        spriteHeight = _renderer.bounds.size.y;
	}

    public Sprite GetSprite()
    {
        return _renderer.sprite;
    }

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag(Constants.TAG_PLAYER)) {
            PlayerPrototype pp = other.GetComponent<PlayerPrototype>();
            if(pp != null)
            {
                if (pp.state == PlayerPrototype.PlayerState.Dead)
                    return;
            }

            if(audioManager)
                audioManager.Play(sound);

            collectibleManager.AddCollectible(this);
        }
	}
}
