using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAds : MonoBehaviour ,IUnityAdsInitializationListener, IUnityAdsLoadListener, AdNetwork, IUnityAdsShowListener
{
    private string androidInterstitialId = "Interstitial_Android";
    private string androidRewardedAdId = "Rewarded_Android";
    private string iosInterstitialId = "Interstitial_iOS";
    private string iosRewardedId = "Rewarded_iOS";
    
    private string interstitialId, rewardedAdId;
    
    private string androidId = "5337974";
    private string iosId = "5337975";
    [SerializeField] bool _testMode = true;
    private string gameId;

    private bool isInterstetialReady, isRewardedReady = false;
    
    private void Awake()
    {
        interstitialId = (Application.platform == RuntimePlatform.Android) ? androidInterstitialId : iosInterstitialId;
        rewardedAdId = (Application.platform == RuntimePlatform.Android) ? androidRewardedAdId : iosRewardedId;
    }
    private void Start()
    {
        Initialize();
    }
 
    private void Initialize()
    {
        gameId = androidId;
        if (Application.platform == RuntimePlatform.Android)
            gameId = androidId;
        
        else
            gameId = iosId;
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, _testMode, this);
        }
    }

 
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        
        LoadInterstitial();
        LoadRewarded();
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    private void LoadInterstitial()
    {
        Advertisement.Load(interstitialId, this);
    }

    private void LoadRewarded()
    {
        Advertisement.Load(rewardedAdId, this);   
    }
    
 
    // Show the loaded content in the Ad Unit:
    public bool ShowInterstitial()
    {
        if (isInterstetialReady)
        {
            isInterstetialReady = false;
            Advertisement.Show(interstitialId, this);
            return true;
        }
        
        return false;
    }

    public bool ShowRewarded()
    {
        if (isRewardedReady)
        {
            isRewardedReady = false;
            Advertisement.Show(rewardedAdId, this);
            return true;
        }
        return false;
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId == interstitialId)
            isInterstetialReady = true;
        
        else if (adUnitId == rewardedAdId)
            isRewardedReady = true;
    }
 
    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Unity Ads Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        Advertisement.Load(_adUnitId, this);
    }
 
    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        Advertisement.Load(_adUnitId, this);
    }
 
    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }

    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Advertisement.Load(_adUnitId, this);
        if(_adUnitId == rewardedAdId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            Ads.Instance.RewardPlayer();
    }
}
