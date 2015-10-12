using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Platform))]
public class DestroyablePlatform : MonoBehaviour
{                                                      
    protected Platform _platform;
    protected bool wasFalling = false;
    protected bool enterInAir = false;

    void Awake()
    {
        _platform = GetComponent<Platform>();
        var collider = GetComponent<BoxCollider2D>();
        collider.offset = new Vector2(collider.offset.x, collider.offset.y + 0.5f);
        collider.size = new Vector2(collider.size.x, collider.size.y * 2);
    }           

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {   
            PlayerPrototype player = other.gameObject.GetComponent<PlayerPrototype>();
            Debug.Log("ENTERED - state:" + player.state);

            if (player.state == PlayerPrototype.PlayerState.Falling)
            { 
                wasFalling = true;
                return;
            }
            else if(player.state == PlayerPrototype.PlayerState.InAir)
            {
                enterInAir = true; 
            }
            wasFalling = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!wasFalling && enterInAir)
            {
                Debug.Log("entered in air");
                PlayerPrototype player = other.gameObject.GetComponent<PlayerPrototype>();
                if (player.state == PlayerPrototype.PlayerState.Falling)
                    wasFalling = true;
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(wasFalling)
            {
                Debug.Log("was falling");
                PlayerPrototype player = other.gameObject.GetComponent<PlayerPrototype>();
                if(other.transform.position.y + player.halfHeight >= transform.position.y + _platform.platformHeight / 2)
                {
                    if(other.transform.position.x + player.halfWidth >= transform.position.x - _platform.platformWidth/2 && other.transform.position.x - player.halfWidth <= transform.position.x + _platform.platformWidth / 2)
                    {
                        _platform.SetKinematic(false);     
                    }
                }
            }
            enterInAir = false;
            wasFalling = false;
        }
    }

}
