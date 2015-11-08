using UnityEngine;
using System.Collections;

public class SwapControlsCollectible : Collectible {

    [SerializeField]
    protected float valueMax = 20;

    [SerializeField]
    protected float valueMin = 10;

    public float value;

    void Awake()
    {
        value = Random.Range(valueMin, valueMax);
    }
}
