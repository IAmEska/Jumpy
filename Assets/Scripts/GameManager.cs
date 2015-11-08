using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AdsManager),typeof(GameServiceManager))]
public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		MainMenu,
		Reset,
		Start,
		Game,
		Paused,
		Killed
	}

	public LevelGenerator levelGenerator;
	public PlayerPrototype player;
	public float _playerYOffset;
	public Destructor[] destructors;
         
    public InputManager inputManager;     
    

	protected CameraFollow _camera;
	protected PlayerPrototype _playerInstance;
	protected Vector3 _spawnPosition;

	protected GameState  _state, _prevState;
    protected int _maxPlayerPositionY;

    protected GameServiceManager _gameServiceManager;
    protected AdsManager _adsManager;
    protected SettingsManager _settingsManager;
    protected CollectibleManager _collectibleManager;

    public Canvas GUI_mainMenu, GUI_inGame;
    public UnityEngine.UI.Text GUI_scoreBoard;


    public void DoDoubleJump()
    {
        inputManager.performDoubleJump = true;
    }

    // Use this for initialization
    void Start()
    {
        _adsManager = GetComponent<AdsManager>();
        _settingsManager = GetComponent<SettingsManager>();
        _collectibleManager = GetComponent<CollectibleManager>();

        _state = GameState.MainMenu;
		GUI_mainMenu.gameObject.SetActive (true);
        GUI_inGame.gameObject.SetActive(false);

		_camera = Camera.main.GetComponent<CameraFollow> ();
		_spawnPosition = _camera.transform.position;
		_spawnPosition.y -= Camera.main.orthographicSize - _playerYOffset;
		_spawnPosition.z = 0;
        

        _adsManager.CreateAdBanner ();

		for (int i=0; i< destructors.Length; i++) {
			destructors [i].Reset += OnReset;
		}

        _gameServiceManager = GetComponent<GameServiceManager>();
        _gameServiceManager.Init();
        _gameServiceManager.SignIn();

        GameSettings.sensitivity = PlayerPrefs.GetFloat(Constants.SETTINGS_SENSITIVITY, 1);
    }

	public void OnReset ()
	{
        GameSettings.game_lastScore = _maxPlayerPositionY;
        if(GameSettings.game_bestScore < _maxPlayerPositionY)
        {
            GameSettings.game_bestScore = _maxPlayerPositionY;
            _gameServiceManager.SubmitScore(_maxPlayerPositionY);
        }

        _settingsManager.SetIndicators();

         _maxPlayerPositionY = 0;
        GUI_scoreBoard.text = "" + _maxPlayerPositionY;        
		_state = GameState.Killed;
	}

	public void OnStartGame ()
	{
		_state = GameState.Reset;
		GUI_mainMenu.gameObject.SetActive (false);
        GUI_inGame.gameObject.SetActive(true);

        if(_playerInstance == null)
        { 
            _playerInstance = Instantiate(player, _spawnPosition, Quaternion.identity) as PlayerPrototype;
            _collectibleManager.playerPrototype = _playerInstance;
            _camera.player = _playerInstance;
        }
    }           

	// Update is called once per frame
	void FixedUpdate ()
	{
		switch (_state) {
		
		case GameState.MainMenu:
			if (_prevState != _state)
            { 
				GUI_mainMenu.gameObject.SetActive (true);
                GUI_inGame.gameObject.SetActive(false);
            }
            break;

		case GameState.Reset:    
			_playerInstance.Reset ();
                
             inputManager.Reset();
			_camera.Reset ();
			_state = GameState.Start;
			break;
                
		case GameState.Start:                                                       
			_state = GameState.Game;
            _adsManager.HideAd(); 
            levelGenerator.SetState(LevelGenerator.GeneratorState.START);
			_playerInstance.SetState (PlayerPrototype.PlayerState.Falling);
            break;

		case GameState.Game:
            if (_playerInstance.state == PlayerPrototype.PlayerState.Dead)
            {
                levelGenerator.SetState(LevelGenerator.GeneratorState.DEAD);
            }         

            if((int)_playerInstance.transform.position.y > _maxPlayerPositionY)
            {    
                _maxPlayerPositionY = (int)_playerInstance.transform.position.y;
                _gameServiceManager.SetScore(_maxPlayerPositionY);
                GUI_scoreBoard.text = "" + _maxPlayerPositionY;
            }

            inputManager.HandleInput();

            if(inputManager.performDoubleJump)
            {
                inputManager.performDoubleJump = false;
                 _playerInstance.DoubleJump();
            }

			break;
		case GameState.Killed:
            //TODO menu blabla
            if (_prevState != _state)
            {
                levelGenerator.SetState(LevelGenerator.GeneratorState.RESET);
                _collectibleManager.SetState(CollectibleManager.ManagerState.RESET);
                GUI_mainMenu.gameObject.SetActive(true);
                GUI_inGame.gameObject.SetActive(false);
                _adsManager.ShowAd();
            }
            //_state = GameState.Reset;
            break;
		}

		_prevState = _state;
	}
              

	
}
