using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

	public Transform followObject;
	public float followSpeed = 5f;
	public float zOffset = -10;

	protected bool _follow = false;
	protected Vector3 _startPosition;


	void Start ()
	{
		_startPosition = transform.position;
	}

	void FixedUpdate ()
	{
		if (followObject != null) {
			_follow = followObject.position.y > transform.position.y;
		}
	}

	void LateUpdate ()
	{
		if (_follow) {
			Vector3 pos = transform.position;
			float y = pos.y;
			y = Mathf.Lerp (y, followObject.position.y, followSpeed * Time.deltaTime);
			pos.y = y;
			transform.position = pos;
		}
	}

	public void Reset ()
	{
		transform.position = _startPosition;
		_follow = false;
	}
}
