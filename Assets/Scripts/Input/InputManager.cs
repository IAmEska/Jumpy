using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public TrailRenderer touchEffect;
    public LayerMask[] touchLayers;
    public float touchSize = 1.5f;

    protected int _touchId;
    protected int _touchLayersMask;

    void Start()
    {
        foreach (LayerMask mask in touchLayers)
        {
            _touchLayersMask |= mask.value;
        }
    }

     protected void CheckIfHit(Touch t)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(t.position);
        var pos1 = pos;
        pos1.x -= touchSize / 2;
        var pos2 = pos1;
        pos2.x += touchSize;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, touchSize, _touchLayersMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                var enemy = collider.GetComponent<Enemy>();
                if (enemy != null && enemy.status == Enemy.Status.Alive)
                {
                    if (GameSettings.vibration)
                        Handheld.Vibrate();

                    enemy.Hit();
                }
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
