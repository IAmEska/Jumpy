using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Platform))]
[RequireComponent(typeof(BoxCollider2D))] 
public class MovingPlatform : MonoBehaviour {
                                 
    public float speed = 3f;
    public float offsetX = 1f;

    protected int direction = 1;
    protected Platform _platform;

    void Awake()
    {
        _platform = GetComponent<Platform>();
        
    }


    void FixedUpdate()
    {
        Vector3 nextPos = transform.position + transform.right * direction * speed * Time.deltaTime;

        if ((nextPos.x < (_platform.minPosX + _platform.platformWidth/2) && direction == -1) || (nextPos.x > (_platform.maxPosX - _platform.platformWidth/2) && direction == 1))
        {                                                                   
            direction *= -1;
            return;
        }

        transform.position = nextPos;

        var hits = Physics2D.RaycastAll(nextPos, transform.right * direction, _platform.platformWidth/2, 1 <<  gameObject.layer);
        bool wasHit = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                {
                    wasHit = true;
                    break;
                }
            }
        }

        if (!wasHit)
        {
            transform.position = nextPos;
        }
        else
        {         
            direction *= -1;
        }  
    }
}
