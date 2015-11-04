using UnityEngine;
using System.Collections;

public class CollectibleManager : MonoBehaviour {      

    public PlatformFactory platformFactory;         

    protected CollectiblePlatformManager collectiblePlatformManager;

    void Start()
    {
        collectiblePlatformManager = new CollectiblePlatformManager(platformFactory);
    }

    public void AddCollectible(Collectible collectible)
    {                                                  
        if(collectible is CollectiblePlatforms)
        {
            Debug.Log("add collectible platfrom");
            CollectiblePlatforms cp = collectible as CollectiblePlatforms;
            AddPlatformCollectible(cp.type, cp.value);
        }

        Destroy(collectible.gameObject);
    }

	// Use this for initialization
	void AddPlatformCollectible(PlatformTypeChance.PlatformType type, int value)
    {
        int positionYTo = (int)Camera.main.transform.position.y + value;
        Debug.Log("to position y : " + positionYTo);
        collectiblePlatformManager.AddCollectible(type, positionYTo);
    }        

    public void Reset()
    {
        collectiblePlatformManager.Clear(); 
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        collectiblePlatformManager.CheckCollectibles(Camera.main.transform.position.y);
    }
}
