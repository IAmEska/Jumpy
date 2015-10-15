using UnityEngine;
using System.Collections;

public class GameServiceManager : MonoBehaviour {

    const int TEST_ACHIEVMENT_SCORE = 10;

    protected int _score;
        
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

    }
}
