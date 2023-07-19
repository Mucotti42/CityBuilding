using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using Random = UnityEngine.Random;

public enum AdType
{
    INTERSTITIAL,
    REWARDED,
}

public enum RewardType
{
    Gold,
}

public class Ads : MonoBehaviour
{
    public static Ads Instance;
    public RewardType rewardType;

    private List<AdNetwork> Networks = new List<AdNetwork>();

    private int index = 0;
    private void Awake()
    {
        if (!Instance) Instance = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            Networks.Add(transform.GetChild(i).GetComponent<AdNetwork>());
        }
    }

    private void Start()
    {
        StartCoroutine(TestCoruotine2());
    }

    private IEnumerator TestCoruotine()
    {
        yield return new WaitForSeconds(3);
        ShowAds(Random.Range(0,2) == 0 ? AdType.REWARDED : AdType.INTERSTITIAL);
        StartCoroutine(TestCoruotine());
    }
    private IEnumerator TestCoruotine2()
    {
        yield return new WaitForSeconds(7);
        if (Random.Range(0, 2) == 0)
            Networks[index%3].ShowInterstitial();
        else
            Networks[index%3].ShowRewarded();
        index++;
        StartCoroutine(TestCoruotine2());
    }

    public void ShowAds(AdType type, RewardType reward = RewardType.Gold)
    {
        switch (type)
        {
            case AdType.INTERSTITIAL:
            {
                ShowInterstitial();
                return;
            }
            case AdType.REWARDED:
            {
                ShowRewarded(reward);
                return;
            }
        }
    }
        
    private void ShowInterstitial()
    {
        for (int i = 0; i < Networks.Count; i++)
        {
            if (Networks[i].ShowInterstitial())
            {
                Debug.Log("Interstitial showed " + i);
                return;
            }
        }
        Debug.Log("No network managed to show interstitial");
    }

    private void ShowRewarded(RewardType type)
    {
        this.rewardType = type;
        for (int i = 0; i < Networks.Count; i++)
        {
            if (Networks[i].ShowRewarded())
            {
                Debug.Log("Rewarded showed " + i);
                return;
            }
        }
        Debug.Log("No network managed to show interstitial");
    }

    public void RewardPlayer()
    {
        //TODO Reward Player
        Debug.Log($"Player rewarded with {rewardType.ToString()}.");
    }
}

public interface AdNetwork
{
    public bool ShowInterstitial();
    public bool ShowRewarded();
}