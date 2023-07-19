using System;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using GoogleMobileAds.Mediation.AdColony.Api;
using AdColonyAppOptions = GoogleMobileAds.Api.Mediation.AdColony.AdColonyAppOptions;

public class AdmobBidding : MonoBehaviour, AdNetwork
{
    private string interstitialId, rewardedAdId;
    
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    private AdRequest adRequest;

    public void Start()
    {
        Configs();
        Initialize();
        
        LoadInterstitial();
        LoadRewarded();
    }

    private void Configs()
    {
        AdColonyAppOptions.SetPrivacyFrameworkRequired(AdColonyPrivacyFramework.GDPR, true);
        AdColonyAppOptions.SetPrivacyConsentString(AdColonyPrivacyFramework.GDPR, "myPrivacyConsentString");   
        
        AdColonyAppOptions.SetPrivacyFrameworkRequired(AdColonyPrivacyFramework.CCPA, true);
        AdColonyAppOptions.SetPrivacyConsentString(AdColonyPrivacyFramework.CCPA, "myPrivacyConsentString");
    }
    private void Initialize()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            interstitialId = "ca-app-pub-5557366454045726/5548643172";
            rewardedAdId = "ca-app-pub-1131363594533119/8930094499";
        }
        else
        {
            interstitialId = "ca-app-pub-1131363594533119/5773701708";
            rewardedAdId = "ca-app-pub-1131363594533119/2037383087";
        }
        
        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        Debug.Log("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        Debug.Log("Adapter: " + className + " is initialized.");
                        break;
                }
            }
        });
        adRequest = new AdRequest.Builder().Build();
    }

    private void LoadInterstitial()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        
        InterstitialAd.Load(interstitialId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());
                
                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
    }
    private void LoadRewarded()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        
        RewardedAd.Load(rewardedAdId, adRequest,
            (RewardedAd  ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());
                
                rewardedAd = ad;
                RegisterEventHandlers(rewardedAd);
            });
    }

    public bool ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
            return true;
        }
        
        Debug.LogError("Interstitial ad is not ready yet.");
        return false;
    }
    public bool ShowRewarded()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Ads.Instance.RewardPlayer();
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
            return true;
        }

        return false;
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            
            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitial();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            
            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitial();
        };
    }
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            
            // Reload the ad so that we can show another as soon as possible.
            LoadRewarded();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            
            // Reload the ad so that we can show another as soon as possible.
            LoadRewarded();
        };
    }
}
