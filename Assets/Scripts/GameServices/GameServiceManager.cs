using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GameServiceManager : MonoBehaviour {

    const int TEST_ACHIEVMENT_SCORE = 10;
    public const string LEADERBOARD_ID = "CgkIhc_Q_OkGEAIQCQ";

    protected int _score;
    protected bool _hasAchievment = false;


    public UnityEngine.UI.Text myText;

    

    void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    public void SignIn()
    {                                 

        myText.text = "SignIn Called\n";

        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                myText.text += "logged\n";
                Debug.Log("logged");
                string userInfo = "Username: " + Social.localUser.userName +
                "\nUser ID: " + Social.localUser.id +
                "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);
                Social.LoadAchievements((IAchievement[] achievements) =>
                {
                    if (achievements.Length == 0)
                    {
                        myText.text += "No achievements\n"; 
                    }
                    else
                    {
                        myText.text += "Have achievements\n"; 
                    }
                });
            }
            else
            {                              
                myText.text += "not logged\n";
            }
            
        });
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
        
    public void ShowAchievement()
    {
        Social.ShowAchievementsUI();
    }

    public void SubmitScore(int score)
    {
        if(Social.localUser.authenticated)
        {
            Social.ReportScore(score, LEADERBOARD_ID, (bool response) =>
            {
                if(response)
                {
                    //Debug.Log("score submited yay");
                }
            });
        }
    }

	public void SetScore(int score)
    {
        if(_score != score)
        { 
            _score = score;
            if(Social.localUser.authenticated)
            {
                
                if (TEST_ACHIEVMENT_SCORE <= score && !_hasAchievment)
                {
                    _hasAchievment = true;
                    myText.text += "Pripojeny\n";
                    ShowTestAchievement();
                }
            }
            else
            {
                myText.text += "Ani kokot\n";
            }
        }
    }

    protected void ShowTestAchievement()
    {
        //Debug.Log("calledAchievement");
        myText.text += "calledAchievement\n";
        Social.ReportProgress(Constants.achievement_firstten, 100.0f, (bool success) =>
        {
            if (success)
            {
                myText.text += "jo\n";
                //Debug.Log("testAchievement");
            }
            else
            {
                myText.text += "picu\n";
                //Debug.Log("testAchievement failed");
            }
        });
        //Social.ShowAchievementsUI();
    }

    public void Init() 
    {
        // PlayGamesPlatform.DebugLogEnabled = true;
        
    }
}
