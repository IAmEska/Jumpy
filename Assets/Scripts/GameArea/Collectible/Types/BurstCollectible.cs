using UnityEngine;
using System.Collections;

public class BurstCollectible : Collectible
{

    [SerializeField]
    protected float valueMax = 200;

    [SerializeField]
    protected float valueMin = 100;

    public float value;

    void Awake()
    {
        value = Random.Range(valueMin, valueMax);
    }
}
