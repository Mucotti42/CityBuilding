using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceAds : MonoBehaviour, AdNetwork
{
    private string AppKey = "1ac073b7d";
    private string InterstitialPlacement = "Interstitial";
    private string RewardedPlacement = "Rewarded";

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            AppKey = "1ac077505";
        
        IronSource.Agent.validateIntegration();
    }

    private void Start()
    {
        IronSource.Agent.init(AppKey);

        // Interstitial reklamın yüklendiğinde tetiklenecek callback
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReady;

        // Rewarded reklamın yüklendiğinde tetiklenecek callback
        IronSourceEvents.onRewardedVideoAdReadyEvent += RewardedAdLoaded;

        // Rewarded reklam tamamlandığında tetiklenecek callback
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedAdCompleted;

        LoadInterstitialAd();
        LoadRewardedAd();
    }

    public bool ShowInterstitial()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial(InterstitialPlacement);
            return true;
        }
        Debug.Log("Interstitial reklam hazır değil.");
        return false;
    }

    public bool ShowRewarded()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo(RewardedPlacement);
            return true;
        }
        
        Debug.Log("Rewarded reklam hazır değil.");
        return false;
    }

    private void LoadInterstitialAd()
    {
        IronSource.Agent.loadInterstitial();
    }

    private void LoadRewardedAd()
    {
        IronSource.Agent.loadRewardedVideo();
    }

    private void InterstitialAdReady()
    {
        Debug.Log("Interstitial reklam yüklendi.");
    }

    private void RewardedAdLoaded()
    {
        Debug.Log("Rewarded reklam yüklendi.");
    }

    private void RewardedAdCompleted(IronSourcePlacement placement)
    {
        Debug.Log("Rewarded reklam tamamlandı. Verilen ödül: " + placement.getRewardAmount());
        Ads.Instance.RewardPlayer();
    }
}
