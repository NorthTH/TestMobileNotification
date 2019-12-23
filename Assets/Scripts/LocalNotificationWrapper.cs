#if UNITY_ANDROID
using Unity.Notifications.Android;

#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

namespace LocalNotificationManager
{
    public class LocalNotificationWrapper
    {
        private bool isInitialized;

#if UNITY_ANDROID
        AndroidNotification m_androidNotification;
        int notiId;
        string ChannelId;
        string ChannelName;
        string Description;
#endif

#if UNITY_IOS
        iOSNotification m_iOSNotification;
#endif

        public LocalNotificationWrapper(string channelId = "channelId",string channelName = "Default ChannelName", string description = "Channel Description")
        {
#if UNITY_ANDROID
            ChannelId = channelId;
            ChannelName = channelName;
            Description = description;
#endif
        }

        public void SetNotification(string title,string body,int afterSec)
        {
            Initialize();
            CancelNotification();
#if UNITY_ANDROID
            // 通知を送信する
            m_androidNotification =   new AndroidNotification
            {
                Title = title,
                Text = body,
                // アイコンをそれぞれセット
                SmallIcon = "push_icon",
                LargeIcon = "title_icon",
                // 今から何秒後に通知をするか？
                FireTime = System.DateTime.Now.AddSeconds(afterSec)
            };
            notiId = AndroidNotificationCenter.SendNotification(m_androidNotification, ChannelId);
#endif

#if UNITY_IOS


            m_iOSNotification = new iOSNotification()
            {
                Title = title,
                Body = body,
                ShowInForeground = true,
                Badge = 1,
                // 時間をトリガーにする
                Trigger = new iOSNotificationTimeIntervalTrigger()
                {
                    TimeInterval = new System.TimeSpan(0, 0, afterSec),
                    Repeats = false
                }
            };
            iOSNotificationCenter.ScheduleNotification(m_iOSNotification);
#endif
        }

        public void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;
#if UNITY_ANDROID
            // 通知チャンネルの登録
            AndroidNotificationCenter.RegisterNotificationChannel(
                new AndroidNotificationChannel
                {
                    Id = ChannelId,
                    Name = ChannelName,
                    Importance = Importance.High,
                    Description = Description,
                });
#endif
        }

        public void CancelAllNotification()
        {
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllScheduledNotifications();
#endif

#if UNITY_IOS
            iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif
        }

        public void CancelNotification()
        {
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelScheduledNotification(notiId);
#endif

#if UNITY_IOS
            iOSNotificationCenter.RemoveScheduledNotifications(m_iOSNotification.Identifier);
#endif
        }

        public void RemoveAllDisplayedNotifications()
        {
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
#endif

#if UNITY_IOS
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif
        }
    }
}
