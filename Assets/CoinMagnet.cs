using UnityEngine;
using System.Collections;

public class CoinMagnet : MonoBehaviour
{
    public float magnetPower = 5f;

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag(Constants.TAG_COIN))
        {
            //Vector3 dir =  transform.position - other.transform.position;
            //Debug.Log("Dir:" + dir);
            other.transform.position = Vector3.Lerp(other.transform.position, transform.position, Time.deltaTime * magnetPower);
        }
    }

}
