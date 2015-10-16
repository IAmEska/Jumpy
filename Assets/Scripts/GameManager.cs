using UnityEngine;
using System.Collections;

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
    public Canvas mainMenu;     
    public InputManager inputManager;
    public AdsManager adsManager;
    public UnityEngine.UI.Text scoreBoard;

	protected CameraFollow _camera;
	protected PlayerPrototype _playerInstance;
	protected Vector3 _spawnPosition;
	protected GameState  _state, _prevState;
    protected int _maxPlayerPositionY;
    protected GameServiceManager _gameServiceManager;

	// Use this for initialization
	void Start ()
	{
        
        _gameServiceManager = GetComponent<GameServiceManager>();
        _gameServiceManager.Init();
        _gameServiceManager.SignIn();

        _state = GameState.MainMenu;
		mainMenu.gameObject.SetActive (true);

		_camera = Camera.main.GetComponent<CameraFollow> ();
		_spawnPosition = _camera.transform.position;
		_spawnPosition.y -= Camera.main.orthographicSize - _playerYOffset;
		_spawnPosition.z = 0;
		_playerInstance = Instantiate (player, _spawnPosition, Quaternion.identity) as PlayerPrototype;
        _camera.player = _playerInstance;

		adsManager.CreateAdBanner ();

		for (int i=0; i< destructors.Length; i++) {
			destructors [i].Reset += OnReset;
		}
	}

	public void OnReset ()
	{
        scoreBoard.text = "" + _maxPlayerPositionY;        
		_state = GameState.Killed;
	}

	public void OnStartGame ()
	{
		_state = GameState.Reset;
		mainMenu.gameObject.SetActive (false);
	}           

	// Update is called once per frame
	void FixedUpdate ()
	{
		switch (_state) {
		
		case GameState.MainMenu:
			if (_prevState != _state)
				mainMenu.gameObject.SetActive (true);
			break;

		case GameState.Reset:    
			_playerInstance.Reset ();
			levelGenerator.Reset ();
                inputManager.Reset();
			_camera.Reset ();
			adsManager.HideAd();
			_state = GameState.Start;
			break;
		case GameState.Start:
			_state = GameState.Game;
			levelGenerator.generate = true;
			_playerInstance.SetState (PlayerPrototype.PlayerState.Falling);
			break;
		case GameState.Game:
            if (_playerInstance.state == PlayerPrototype.PlayerState.Idle)
            {
                levelGenerator.SetDeadStatus();
            }

            if((int)_playerInstance.transform.position.y > _maxPlayerPositionY)
            {    
                _maxPlayerPositionY = (int)_playerInstance.transform.position.y;
                _gameServiceManager.SetScore(_maxPlayerPositionY);
                scoreBoard.text = "" + _maxPlayerPositionY;
            }
            inputManager.HandleInput();
			break;
		case GameState.Killed:
            //TODO menu blabla
            if (_prevState != _state)
            {
                mainMenu.gameObject.SetActive(true);
                adsManager.ShowAd();
            }
            //_state = GameState.Reset;
            break;
		}

		_prevState = _state;
	}
              

	
}
