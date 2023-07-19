using System;
// using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
// using System.Collections;
   using GooglePlayGames;
   using UnityEngine;

public class GPGSManager : MonoBehaviour
{
     private void Start()
     {
         //PlayGamesPlatform.Activate();
         //Social.localUser.Authenticate(ProcessAuthentication);
         
         //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
         // PlayGamesPlatform.Instance.UnlockAchievement();
     }
    
     internal void ProcessAuthentication(SignInStatus status) {
         if (status == SignInStatus.Success) {
             // Continue with Play Games Services
         } else {
             // Disable your integration with Play Games Services or show a login button
             // to ask users to sign-in. Clicking it should call
             // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
         }
     }
     internal void ProcessAuthentication(bool status) {
         if (status) {
             // Continue with Play Games Services
         } else {
             // Disable your integration with Play Games Services or show a login button
             // to ask users to sign-in. Clicking it should call
             // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
         }
     }
}
