using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public abstract class Collectible : MonoBehaviour
{
    public AudioClip audioClip;
    public CollectibleManager collectibleManager;

	protected AudioSource _audioSource;

	void Start ()
	{
		_audioSource = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {
			if (GameSettings.vibration)
				Handheld.Vibrate ();

			if (GameSettings.soundFx) {
                Debug.Log("DING");
                _audioSource.volume = 1;
				_audioSource.PlayOneShot (audioClip, GameSettings.soundFxVolume);
            }

            collectibleManager.AddCollectible(this);
        }
	}
}
