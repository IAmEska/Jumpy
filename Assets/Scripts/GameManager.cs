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
	public Canvas mainMenu, endMenu;
	public float touchSize = 1.5f;
	public LayerMask[] touchLayers;

	protected CameraFollow _camera;
	protected PlayerPrototype _playerInstance;
	protected Vector3 _spawnPosition;
	protected GameState  _state, _prevState;
	protected int _touchLayersMask;

	// Use this for initialization
	void Start ()
	{
		_state = GameState.MainMenu;
		mainMenu.gameObject.SetActive (true);
		endMenu.gameObject.SetActive (false);

		_camera = Camera.main.GetComponent<CameraFollow> ();
		_spawnPosition = _camera.transform.position;
		_spawnPosition.y -= Camera.main.orthographicSize - _playerYOffset;
		_spawnPosition.z = 0;
		_playerInstance = Instantiate (player, _spawnPosition, Quaternion.identity) as PlayerPrototype;
		_camera.followObject = _playerInstance.transform;

		for (int i=0; i< destructors.Length; i++) {
			destructors [i].Reset += OnReset;
		}

		foreach (LayerMask mask in touchLayers) {
			_touchLayersMask |= mask.value;
		}
	}

	public void OnReset ()
	{
		_state = GameState.Killed;
	}

	public void OnStartGame ()
	{
		_state = GameState.Reset;
		mainMenu.gameObject.SetActive (false);
	}

	public void OnResetGame ()
	{
		_state = GameState.Reset;
		endMenu.gameObject.SetActive (false);
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
			_camera.Reset ();
			_state = GameState.Start;
			break;
		case GameState.Start:
			_state = GameState.Game;
			levelGenerator.generate = true;
			_playerInstance.SetState (PlayerPrototype.PlayerState.Falling);
			break;
		case GameState.Game:
			HandleInput ();
			break;
		case GameState.Killed:
			//TODO menu blabla
			if (_prevState != _state)
				endMenu.gameObject.SetActive (true);
			//_state = GameState.Reset;
			break;
		}

		_prevState = _state;
	}

	protected void HandleInput ()
	{
		if (Input.touchCount > 0) {
			Debug.Log ("I ve got touches");
			foreach (Touch t in Input.touches) {
				if (t.phase == TouchPhase.Began) {
					Vector3 pos = Camera.main.ScreenToWorldPoint (t.position);
					var pos1 = pos;
					pos1.x -= touchSize / 2;
					var pos2 = pos1;
					pos2.x += touchSize;
					Debug.DrawLine (pos1, pos2, Color.red, 5);
					Collider2D[] colliders = Physics2D.OverlapCircleAll (pos, touchSize, _touchLayersMask);
					foreach (Collider2D collider in colliders) {
						if (collider.tag == "Enemy") {
							var enemy = collider.GetComponent<Enemy> ();
							if (enemy != null) {
								if (GameSettings.vibration)
									Handheld.Vibrate ();

								enemy.Hit ();
							}
						}
					}
				}
			}
		}
	}
}
