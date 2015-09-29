using UnityEngine;
using System.Collections;

public class MovingPlatform : Platform {

    public static float minX, maxX;

    public float speed;
    public float offsetX;

    protected int direction = 1;

    public void FixedUpdate()
    {

        Vector3 nextPos = transform.position + transform.right * direction * speed * Time.deltaTime;
        Debug.Log("minX:" + minX);
        Debug.Log("maxX:" + maxX);
        if (nextPos.x < (minX + offsetX) || nextPos.x > (maxX - offsetX))
        {
            direction *= -1;
            return;
        }

        var hit = Physics2D.Raycast(nextPos, transform.right * direction, offsetX, 1 << gameObject.layer);
        if (hit.transform == null)
        {
            transform.position = nextPos;
        }
        else
        {
            direction *= -1;
        }
    }
}
