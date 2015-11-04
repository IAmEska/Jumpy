using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Platform))]
[RequireComponent(typeof(BoxCollider2D))]
public class DestroyablePlatform : PlatformComponent
{                                 
    protected bool wasFalling = false;
    protected bool enterInAir = false;

    protected override void Awake()
    {
        base.Awake();
        var collider = GetComponent<BoxCollider2D>();
        collider.offset = new Vector2(collider.offset.x, collider.offset.y + 0.5f);
        collider.size = new Vector2(collider.size.x, collider.size.y * 2);
        _platform.SetSprite(1);
    }           

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {   
            PlayerPrototype player = other.gameObject.GetComponent<PlayerPrototype>();      

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
