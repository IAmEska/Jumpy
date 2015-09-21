using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Platform : MonoBehaviour
{
	public float maxWidth = 3f, minWidth = 1f;
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
