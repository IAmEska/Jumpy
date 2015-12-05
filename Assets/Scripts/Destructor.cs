using UnityEngine;
using System.Collections;

public class Destructor : MonoBehaviour
{

    public int destroyFromHeigh = 5;

	public delegate void ResetFire ();
	public event ResetFire Reset;

	public LevelGenerator levelGenerator;

	public LayerMask destroyLayer;       
                                       

    void OnTriggerEnter2D(Collider2D other)
    {                        
        int objLayerMask = (1 << other.gameObject.layer);
        if (Camera.main.transform.position.y >= destroyFromHeigh)
        {
            if ((destroyLayer.value & objLayerMask) == objLayerMask) {
             levelGenerator.DestroyObject(other.gameObject);
		    }
        }

        if (other.CompareTag(Constants.TAG_PLAYER)) {
			if (Reset != null)
				Reset ();
        }
    }

}
