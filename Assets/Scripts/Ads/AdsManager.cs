﻿using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour {

	#if UNITY_ANDROID
	public const string AD_UNIT_ID = "ca-app-pub-8381349806334522/2207481090";
	#elif UNITY_IPHONE
	public const string AD_UNIT_ID = "ca-app-pub-8381349806334522/6119316699";
	#else
	public const string AD_UNIT_ID = "unexpected_platform";
	#endif

	protected BannerView _bannerView;

	public void CreateAdBanner()
	{
		// Create a 320x50 banner at the top of the screen.
		_bannerView = new BannerView(AD_UNIT_ID, AdSize.Banner, AdPosition.Top);
		
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
			.AddTestDevice(AdRequest.TestDeviceSimulator)
				.AddTestDevice("DAD85FB8131A8028C5BC8470F9B651D5")
                .AddTestDevice("275D5A541089283E92354C982964EC8F")
                .TagForChildDirectedTreatment(false)
				.Build();
		
		// Load the banner with the request.
		_bannerView.LoadAd(request);
	}

    public void ShowAd()
    {
		_bannerView.Show ();
    }

	public void HideAd()
	{
		_bannerView.Hide ();
	}

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Failed:
                break;
        }
    }
}
