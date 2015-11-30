using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour {

    public UnityEngine.UI.Slider sensitivitySlider;
    public GameObject lastScoreIndicator, bestScoreIndicator;

    void Start()
    {
        GameSettings.sensitivity = PlayerPrefs.GetFloat(Constants.SETTINGS_SENSITIVITY, 1);
        GameSettings.game_bestScore = PlayerPrefs.GetInt(Constants.GAME_BEST_SCORE, 0);
        GameSettings.game_lastScore = PlayerPrefs.GetInt(Constants.GAME_LAST_SCORE, 0);
        SetIndicators();

        sensitivitySlider.value = GameSettings.sensitivity;
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    public void SetIndicators()
    {
        if(GameSettings.game_bestScore  > 0)
        {                                                                                                              

            bestScoreIndicator.transform.position = new Vector3(0, GameSettings.game_bestScore, 0);
            lastScoreIndicator.transform.position = new Vector3(0, GameSettings.game_lastScore, 0);

            bestScoreIndicator.gameObject.SetActive(true);
            lastScoreIndicator.gameObject.SetActive(GameSettings.game_bestScore != GameSettings.game_lastScore);    
        }    
    }

	public void SetSensitivity(float value)
    {
        GameSettings.sensitivity = value;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
        {
            PlayerPrefs.SetFloat(Constants.SETTINGS_SENSITIVITY, GameSettings.sensitivity);
            PlayerPrefs.SetInt(Constants.GAME_BEST_SCORE, GameSettings.game_bestScore);
            PlayerPrefs.SetInt(Constants.GAME_LAST_SCORE, GameSettings.game_lastScore);
            PlayerPrefs.Save();
        }
    }
}
