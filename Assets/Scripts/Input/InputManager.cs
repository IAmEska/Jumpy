using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public TrailRenderer touchEffect;
    public LayerMask touchLayers;
    public float touchSize = 1.5f;
    public bool performDoubleJump = false;

    protected int _touchId;             

    void Start()
    {     
    }

     protected void CheckIfHit(Touch t)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(t.position);
        var pos1 = pos;
        pos1.x -= touchSize / 2;
        var pos2 = pos1;
        pos2.x += touchSize;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, touchSize, touchLayers.value);
        foreach (Collider2D collider in colliders)
        {                
            if (collider.CompareTag(Constants.TAG_ENEMY))
            {
                var enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.status == Enemy.Status.Alive)
                {
                    if (GameSettings.vibration)
                        Handheld.Vibrate();

                    if(enemy.hitsToDie > 0)
                        enemy.Hit();
                }
            }
            else if(collider.CompareTag(Constants.TAG_DOUBLE_JUMP))
            {
                performDoubleJump = true;
                //Debug.Log("DOUBLE JUMP HIT");
            }
        }
    }

    protected void SetNewPosition(Touch t)
    {   
        Vector3 pos = Camera.main.ScreenToWorldPoint(t.position);
        pos.z = 1;
        touchEffect.transform.position = pos;
    }
     
    public void Reset()
    {
        _touchId = -1;
    }

    public void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch t in Input.touches)
            {
                if (_touchId == -1)
                {
                    _touchId = t.fingerId;
                    SetNewPosition(t);
                    CheckIfHit(t);
                }
                else if (_touchId == t.fingerId && (t.phase != TouchPhase.Ended && t.phase != TouchPhase.Canceled))
                {
                    SetNewPosition(t);
                    CheckIfHit(t);
                }
                else if(_touchId == t.fingerId)
                {
                    _touchId = -1;
                }

            }
        }
        else if(_touchId != -1)
        {
            _touchId = -1;
        }
    }
         
}
