using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public abstract class Collectible : MonoBehaviour
{
    public AudioClip audioClip;
    public CollectibleManager collectibleManager;
    public float spriteWidth;
    public float spriteHeight;

	protected AudioSource _audioSource;
    protected SpriteRenderer _renderer;

	void Start ()
	{
        _renderer = GetComponent<SpriteRenderer>();
		_audioSource = GetComponent<AudioSource> ();
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
			/*if (GameSettings.vibration)
				Handheld.Vibrate ();

			if (GameSettings.soundFx) {
                Debug.Log("DING");        
				//_audioSource.PlayOneShot (audioClip, GameSettings.soundFxVolume);
            }     */

            collectibleManager.AddCollectible(this);
        }
	}
}
