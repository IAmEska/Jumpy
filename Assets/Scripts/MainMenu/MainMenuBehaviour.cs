using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuBehaviour : MonoBehaviour {

    public RectTransform mainMenu;
    public RectTransform settingsMenu;
    public RectTransform pickerMenu;

    public float transitionTime = 250f;

    protected MenuState _state;
    protected float _defaultSettingsMenuPositionX;
    protected float _defaultMainMenuPositionX;

    public enum MenuState
    {
        MainMenu,
        Settings,
        Picker
    }

	// Use this for initialization
	void Start ()
    {
        _defaultSettingsMenuPositionX = settingsMenu.position.x;
        _defaultMainMenuPositionX = mainMenu.position.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    switch(_state)
        {
            case MenuState.MainMenu:
                MainMenuStateBehaviour();
                break;

            case MenuState.Settings:
                SettingsMenuStateBehaviour();
                break;

            case MenuState.Picker:
                PickerMenuStateBehaviour();
                break;
        }    
	}

    protected void PickerMenuStateBehaviour()
    {
        if (mainMenu.position.x != -_defaultSettingsMenuPositionX)
        {
            MoveMenu(mainMenu, -_defaultSettingsMenuPositionX);
        }

        if (pickerMenu.position.x != _defaultMainMenuPositionX)
        {
            MoveMenu(pickerMenu, _defaultMainMenuPositionX);
        }
    }

    protected void MainMenuStateBehaviour()
    {
        if(mainMenu.position.x != _defaultMainMenuPositionX)
        {
            MoveMenu(mainMenu, _defaultMainMenuPositionX);    
        }

        if(settingsMenu.position.x != _defaultSettingsMenuPositionX)
        {
            MoveMenu(settingsMenu, _defaultSettingsMenuPositionX);
        }

        if(pickerMenu.position.x != _defaultSettingsMenuPositionX)
        {
            MoveMenu(pickerMenu, _defaultSettingsMenuPositionX);
        }
    }

    protected void SettingsMenuStateBehaviour()
    {
        if(mainMenu.position.x != -_defaultSettingsMenuPositionX)
        {
            MoveMenu(mainMenu, -_defaultSettingsMenuPositionX);
        }

        if(settingsMenu.position.x != _defaultMainMenuPositionX)
        {
            MoveMenu(settingsMenu, _defaultMainMenuPositionX);
        }
    }

    protected void MoveMenu(RectTransform menu, float toPositionX)
    {
        Vector3 pos = menu.position;
        float posX = Mathf.SmoothStep(pos.x, toPositionX, Time.deltaTime * transitionTime);
        if (Mathf.Abs(toPositionX - posX) <= 0.02f)
        {
            posX = toPositionX;
        }
        pos.x = posX;
        menu.position = pos;
    }

    public void ShowPickerMenu()
    {
        _state = MenuState.Picker;
    }

    public void ShowSettingsMenu()
    {
        _state = MenuState.Settings;
    }

    public void ShowMainMenu()
    {
        _state = MenuState.MainMenu;
    }
}
