
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

#if UNITY_ANDROID
using Google.Play.Review;
#endif

public class InAppReview : MonoBehaviour
{
    public static InAppReview instance;

    private void Awake()
    {
        instance = this;
    }

#if UNITY_ANDROID
    private ReviewManager reviewManager;
    private PlayReviewInfo _playReviewInfo;

    private void Start()
    {
        reviewManager = new ReviewManager();
    }

    private IEnumerator IERequestReviewInfo()
    {
        var requestFlowOperation = reviewManager.RequestReviewFlow();
        Debug.LogError("Requesting");
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError(requestFlowOperation.Error.ToString());
            yield break;
        }
        Debug.LogError("No Error on request");
        _playReviewInfo = requestFlowOperation.GetResult();
        StartCoroutine(IELaunchInAppReview());
    }

    private IEnumerator IELaunchInAppReview()
    {
        var launchFlowOperation = reviewManager.LaunchReviewFlow(_playReviewInfo);
        Debug.LogError("Launching");
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError(launchFlowOperation.Error.ToString());
            yield break;
        }
            Debug.LogError("No error launch");
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow
    } 
#endif

    public void LaunchInAppReview()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#else
        StartCoroutine(IERequestReviewInfo());
#endif
    }
}
