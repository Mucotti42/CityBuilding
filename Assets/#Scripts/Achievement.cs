using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CloudOnce;
using System;
using CloudOnce;

public class Achievement : MonoBehaviour
{
    int score = 0;

    private void Start()
    {
        Cloud.OnInitializeComplete += CloudInitializeComplete;
        Cloud.Initialize(false, true);
    }

    private void CloudInitializeComplete()
    {
        Cloud.OnInitializeComplete -= CloudInitializeComplete;
        Debug.LogError("Cloud Initialized");
        StartCoroutine(TestScore());
    }
    
    public void IncreaseScore()
    {
        score++;

        if(score == 3)
        {
            Achievements.FirstBuilding.Unlock();
            Debug.Log("Unlocked Beginner");
        }
        
    }

    private IEnumerator TestScore()
    {
        yield return new WaitForSeconds(5f);
        Leaderboards.Richness.SubmitScore(MenuView.instance.gold);
        StartCoroutine(TestScore());
        
    }
}
