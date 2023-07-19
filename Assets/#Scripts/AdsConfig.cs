using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Mediation.AdColony.Api;
using AdColonyAppOptions = GoogleMobileAds.Api.Mediation.AdColony.AdColonyAppOptions;

public class AdsConfig : MonoBehaviour
{
    private void Start()
    {
        AdColonyConfig();
        //TODO ADS ENABLED
        
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void AdColonyConfig()
    {
        AdColonyAppOptions.SetPrivacyFrameworkRequired(AdColonyPrivacyFramework.GDPR, true);
        AdColonyAppOptions.SetPrivacyConsentString(AdColonyPrivacyFramework.GDPR, "myPrivacyConsentString");   
        
        AdColonyAppOptions.SetPrivacyFrameworkRequired(AdColonyPrivacyFramework.CCPA, true);
        AdColonyAppOptions.SetPrivacyConsentString(AdColonyPrivacyFramework.CCPA, "myPrivacyConsentString");
    }
}
