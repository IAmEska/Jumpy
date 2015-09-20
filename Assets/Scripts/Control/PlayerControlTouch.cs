using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerPrototype))]
public class PlayerControlTouch : MonoBehaviour
{

	public float touchAreaHeightPercent = 100 / 3f;
	protected PlayerPrototype _player;
	protected Rigidbody2D _body;

	// Use this for initialization
	void Start ()
	{
		_player = GetComponent<PlayerPrototype> ();
		_body = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector2 dir = Vector2.zero;
		if (Input.touchCount > 0) {
			for (int i=0; i<Input.touchCount; i++) {
				Touch t = Input.touches [i];
				if (t.position.y <= Screen.height / touchAreaHeightPercent) {

					if (t.position.x < Screen.width / 2) {
						dir = -Vector2.right;
					} else {
						dir = Vector2.right;
					}
					break;
				}
			}
		}

		float currentY = _body.velocity.y;
		_body.velocity = dir * _player.forceX + new Vector2 (0, currentY);

	}
}
