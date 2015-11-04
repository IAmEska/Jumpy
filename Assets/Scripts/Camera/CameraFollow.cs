using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

	public PlayerPrototype player;
	public float followSpeed = 2f;
	public float zOffset = -10;
    public float maxReturnY = 10f;

	protected bool _follow = false;
	protected Vector3 _startPosition;
    protected float _actualPosition;


	void Start ()
	{
		_startPosition = transform.position;
        _actualPosition = _startPosition.y;
    }

	void FixedUpdate ()
	{
		if (player != null) {
			_follow = player.state != PlayerPrototype.PlayerState.Dead;
		}
	}

	void LateUpdate ()
	{
        if (_follow) {
            
            Vector3 pos = transform.position;
            float y = pos.y;
            y = Mathf.Lerp(y, player.transform.position.y, followSpeed * Time.deltaTime);
            if (y < _startPosition.y)
                y = _startPosition.y;

            if (_actualPosition - maxReturnY > y)
                y = transform.position.y;

            if(_actualPosition < y)
            {
                _actualPosition = y;
            }
            pos.y = y;
            transform.position = pos;
            
        }
	}

	public void Reset ()
	{
		transform.position = _startPosition;
        _actualPosition = _startPosition.y;
		_follow = false;
	}
}
