using System;
using UnityEngine;

#if UNITY_IOS
using Unity.Notifications.iOS;
#elif UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public enum NotificationTypes
{
    Hungry,
    Bored,
    Play
}
public class Notifications : MonoBehaviour
{
    private void Start()
    {
        Test();
    }

    public void Test()
    {
        SendNotification(NotificationTypes.Play, 0);
    }
    private void OnApplicationQuit()
    {
        SendNotification(NotificationTypes.Play, 1);
    }

    public void SendNotification(NotificationTypes _type, int launchMinute)
    {
        var type = "type";
        var body = "Body";
        var subtitle = "subtitle";

        // IOS
#if UNITY_IOS
        var hours = launchMinute / 60;
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(hours, launchMinute % 60, 0),
            Repeats = false
        };

        var notificationa = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = type,
            Title = "Cat City",
            Body = body,
            Subtitle = subtitle,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notificationa);

        // Android
#elif UNITY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = type,
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var _notification = new AndroidNotification();
        _notification.Title = "Cat City";
        _notification.Text = body + " " + subtitle;
        _notification.FireTime = System.DateTime.Now.AddMinutes(launchMinute);

        // Debug.LogError(body + " " + subtitle);

        AndroidNotificationCenter.SendNotification(_notification, type);
        #endif
    }
}
