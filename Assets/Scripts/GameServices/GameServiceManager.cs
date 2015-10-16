using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class GameServiceManager : MonoBehaviour {

    const int TEST_ACHIEVMENT_SCORE = 10;

    protected bool _isAuthenticated;
    protected int _score;
    
    public void SignIn()
    {
        Debug.Log("SignIn Called");
        Social.localUser.Authenticate((bool success) => {
            _isAuthenticated = success;
            if (_isAuthenticated == true)
            {
                Debug.Log("logged");
                string userInfo = "Username: " + Social.localUser.userName +
                "\nUser ID: " + Social.localUser.id +
                "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);
            }
            else Debug.Log("not logged");
            
        });
    }

        
	public void SetScore(int score)
    {
        if(_score != score)
        { 
            _score = score;
            if(TEST_ACHIEVMENT_SCORE == score)
            {
                ShowTestAchievement();
            }
        }
    }

    protected void ShowTestAchievement()
    {
        Debug.Log("calledAchievement");
        Social.Active.ReportProgress(Constants.achievement_firstten, 10.0f, (bool success) =>
        {
            if (success)
            {
                Debug.Log("testAchievement");
            }
            else Debug.Log("testAchievement failed");
        });
    }

    public void Init() 
    {
        PlayGamesPlatform.Activate();
    }
}
