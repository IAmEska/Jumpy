using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour
{

	public Transform _objectToFollow;
	public float _speed = 2f;
	public float _treshold = 30f;

	protected bool _isMoving = false;

	// Use this for initialization
	void Start ()
	{
	
	}

	void FixedUpdate ()
	{
		if (Mathf.Abs (transform.position.y - _objectToFollow.position.y) >= _treshold) {
			_isMoving = true;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (_isMoving) {
			transform.position += transform.up * _speed * Time.deltaTime;
		}
	}
}
