using UnityEngine;
using System.Collections;

public class Destructor : MonoBehaviour
{

	public delegate void ResetFire ();
	public event ResetFire Reset;

	public LevelGenerator levelGenerator;

	public LayerMask[] destroyLayer;

	protected int _destroyLayer;

	void Start ()
	{
		if (destroyLayer.Length > 0) {
			_destroyLayer = destroyLayer [0];

			for (int i=1; i<destroyLayer.Length; i++) {
				_destroyLayer |= destroyLayer [i].value;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		int objLayerMask = (1 << other.gameObject.layer);
		if ((_destroyLayer & objLayerMask) == objLayerMask) {
			//FIXME cache
			Destroy (other.gameObject);
		} else if (other.tag == "Player") {
			if (Reset != null)
				Reset ();
		}
	}


}
