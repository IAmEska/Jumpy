using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Platform : MonoBehaviour
{

	protected SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Awake ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	public float PlatformHeight {
		get {
			return _spriteRenderer.bounds.size.y;
		}
	}
}
