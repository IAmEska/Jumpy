using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public abstract class Collectible : MonoBehaviour
{
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
				_audioSource.PlayOneShot (_audioSource.clip, GameSettings.soundFxVolume);
			}

			Destroy (gameObject);
		}
	}
}
