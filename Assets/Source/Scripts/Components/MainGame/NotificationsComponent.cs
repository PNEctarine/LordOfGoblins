using Unity.Notifications.Android;
using UnityEngine;

public class NotificationsComponent : MonoBehaviour
{
    private void Start()
    {
        AndroidNotificationChannel channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            var notification = new AndroidNotification
            {
                Title = "HEEEEEEY!!!!",
                Text = "You didn't come in for a 10 hours",
                FireTime = System.DateTime.Now.AddMinutes(60 * 10)
            };

            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }

        else
        {
            AndroidNotificationCenter.DeleteNotificationChannel("channel_id");
        }
    }
}
