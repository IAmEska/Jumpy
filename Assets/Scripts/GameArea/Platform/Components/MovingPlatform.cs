using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Platform))]
[RequireComponent(typeof(BoxCollider2D))] 

public class MovingPlatform : MonoBehaviour {

    public static float minX, maxX;
  
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

        if (nextPos.x < (minX + offsetX) || nextPos.x > (maxX - offsetX))
        {
            direction *= -1;
            return;
        }

        var hits = Physics2D.RaycastAll(nextPos, transform.right * direction, offsetX, 1 <<  gameObject.layer);
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
