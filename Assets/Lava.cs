using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {

	public enum State
	{
		Idle,
		Moving,
		Stop
	}

	public float startMovingFromDistance = 20f;
	public float movingSpeed = 2f;

	protected Vector3 _startPosition;
	protected State _state;

	void Start()
	{
		_startPosition = transform.position;
		_state = State.Idle;
	}
	 
	public void Reset()
	{
		transform.position = _startPosition;
		_state = State.Idle;
	}

	void FixedUpdate () 
	{
		switch (_state) {
		case State.Idle:
			if(Mathf.Abs (transform.position.y - Camera.main.transform.position.y) >= startMovingFromDistance)
			{
				_state = State.Moving;
			}
			break;
		case State.Moving:
			Vector3 newPos = transform.position;
			newPos.y += movingSpeed * Time.deltaTime;
			transform.position = newPos;
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.CompareTag(Constants.TAG_PLAYER))
		{
			PlayerPrototype pp = other.GetComponent<PlayerPrototype>();
			if(pp != null)
			{
				pp.Kill();
			}
			_state = State.Stop;
		}
	}
}