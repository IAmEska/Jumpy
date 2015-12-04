using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

	protected AudioSound _clipToPlay;

	protected AudioSource _audioSource;

    public AudioClip coin, resize, shrink;


    public enum AudioSound
    {
        Nothing,
        Coin,
        Resize,
        Shrink
    }

	// Use this for initialization
	void Start () 
	{
		_audioSource = GetComponent<AudioSource>();
        _clipToPlay = AudioSound.Nothing;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(_clipToPlay)
        {
            case AudioSound.Coin:
                _audioSource.PlayOneShot(coin);
                break;
            case AudioSound.Resize:
                _audioSource.PlayOneShot(resize);
                break;
            case AudioSound.Shrink:
                _audioSource.PlayOneShot(shrink);
                break;
        }
    
        _clipToPlay = AudioSound.Nothing;
	}

	public void Play(AudioSound type)
	{
        _clipToPlay = type;
    }
}