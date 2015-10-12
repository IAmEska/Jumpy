using UnityEngine;          
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
}
