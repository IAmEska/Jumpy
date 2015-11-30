using UnityEngine;
using System.Collections;

public class CollectibleManager : MonoBehaviour {      

    public enum ManagerState
    {
        IDLE,
        ADDCOLLECTIBLE,
        RESET
    }

    protected ManagerState _state;

    public PowerUpInfoManager powerUpInfoManager;
    public PlatformFactory platformFactory;
    public PlayerPrototype playerPrototype;
    public LayerMask landMask;
    public int defaultDuration = 20;

    protected CollectiblePlatformManager _collectiblePlatformManager;
    protected Collectible _collectibleToAdd;
    protected float _swapControlToY;             

    void Awake()
    {
        _state = ManagerState.IDLE;
        _collectiblePlatformManager = new CollectiblePlatformManager(platformFactory);
    }                              

    public void SetState(ManagerState state)
    {
        _state = state;
    }

    public void AddCollectible(Collectible collectible)
    {               
        _collectibleToAdd = collectible;
        _state = ManagerState.ADDCOLLECTIBLE; 
    }

	// Use this for initialization
	void AddPlatformCollectible(PlatformTypeChance.PlatformType type, float value)
    {   
        _collectiblePlatformManager.AddCollectible(type, value);
        

        for (int i=0; i<platformFactory.transform.childCount; i++)
        {
            GameObject child = platformFactory.transform.GetChild(i).gameObject;
            int objectLayer = 1 << child.layer;                                
            if (1 << child.layer == landMask.value)
            {
                Platform p = child.GetComponent<Platform>();
                
                switch (type)
                {
                    default:
                        p.AddPlatformComponent(type);  
                        break;

                    case PlatformTypeChance.PlatformType.Resizeable:
                        p.StartResizeAnimation(value);
                        break; 
                                                             
                 }
                   
            }
        }
    }

    protected void AddSwapControlCollectible(float value)
    {           
        _swapControlToY = value + Camera.main.transform.position.y;
        playerPrototype.isControlSwaped = true;
    }

    protected void AddShieldCollectible()
    {
        playerPrototype.SetShield(true);
    }

    protected void AddBurstCollectible(float value)
    {    
        playerPrototype.BurstJump(value);
    }

    protected void AddCollectibleBehaviour()
    {
        bool addCollectibleToGui = false;

        float value = 0, infoValue = 0;
        if (_collectibleToAdd is AbstractCollectiblePlatforms)
        {                                           
            AbstractCollectiblePlatforms cp = _collectibleToAdd as AbstractCollectiblePlatforms;
            if(cp.type == PlatformTypeChance.PlatformType.Resizeable)
            { 
                value = cp.value;
                infoValue = Camera.main.transform.position.y + CollectiblePlatformManager.DEFAULT_DURATION;
            }
            else
            {
                value = cp.value + Camera.main.transform.position.y;
                infoValue = value;
            }
            AddPlatformCollectible(cp.type, value);
            addCollectibleToGui = true;
        }
        else if(_collectibleToAdd is SwapControlsCollectible)
        {                                
            SwapControlsCollectible scc = _collectibleToAdd as SwapControlsCollectible;
            value = scc.value + Camera.main.transform.position.y;
            infoValue = value;
            AddSwapControlCollectible(value);
            addCollectibleToGui = true;
        }
        else if(_collectibleToAdd is BurstCollectible)
        {
            BurstCollectible bc = _collectibleToAdd as BurstCollectible;
            AddBurstCollectible(bc.value);
        }
        else if(_collectibleToAdd is ShieldCollectible)
        {
            AddShieldCollectible();
        }

        if(addCollectibleToGui)
            powerUpInfoManager.AddCollectible(_collectibleToAdd, infoValue);
        
        Destroy(_collectibleToAdd.gameObject);
    }

    protected void ResetBehaviour()
    {                        
        _collectiblePlatformManager.Clear();
        playerPrototype.isControlSwaped = false;
    }

    protected void CheckControllCollectible()
    {
        if(playerPrototype)
        { 
            if(playerPrototype.isControlSwaped)
            {
                if(_swapControlToY < Camera.main.transform.position.y)
                {
                    playerPrototype.isControlSwaped = false;
                }
            }
        }
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
         switch(_state)
         {
            case ManagerState.ADDCOLLECTIBLE:
                AddCollectibleBehaviour();
                _state = ManagerState.IDLE;
                break;

            case ManagerState.RESET:
                ResetBehaviour();
                powerUpInfoManager.Clear();
                _state = ManagerState.IDLE;
                break;
        }

        _collectiblePlatformManager.CheckCollectibles(Camera.main.transform.position.y);
        CheckControllCollectible();
    }
}
