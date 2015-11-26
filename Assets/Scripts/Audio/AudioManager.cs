using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

	protected List<AudioClip> _clipsToPlay;

	protected AudioSource _audioSource;

	// Use this for initialization
	void Start () 
	{
		_audioSource = GetComponent<AudioSource>();
		_clipsToPlay = new List<AudioClip>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_clipsToPlay.Count > 0)
		{
			foreach(AudioClip clip in _clipsToPlay)
			{
				_audioSource.PlayOneShot(clip);
			}
			_clipsToPlay.Clear();
		}
	}

	public void Play(AudioClip clip)
	{
		_clipsToPlay.Add (clip);
	}
}