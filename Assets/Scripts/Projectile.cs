using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{

	public float lifeTime = 3f;
	public float speed = 100f;
	public LayerMask[] destroyMask;
	public float collideDistance;

	protected SpriteRenderer _renderer;
	protected float _width;
	public int _mask;

	// Use this for initialization
	void Start ()
	{
		_renderer = GetComponent<SpriteRenderer> ();
		_width = _renderer.bounds.size.x;
		for (int i =0; i<destroyMask.Length; i++) {
			_mask |= destroyMask [i].value;
		}


	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		var hit = Physics2D.CircleCast (transform.position, _width, transform.up, collideDistance, _mask);
		if (hit.transform != null) {
			if ((1 << hit.transform.gameObject.layer & _mask) == 1 << hit.transform.gameObject.layer) {
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Enemy")) {
					//hit.transform.GetComponent<Enemy> ().Hit (hit.point);
				}
				Destroy (gameObject);
			}
		}

		transform.position = transform.position + transform.up * speed * Time.deltaTime;
	}
}
