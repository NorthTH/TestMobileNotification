using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalNotificationManager;
using System;

public class NotificationsManager : MonoBehaviour
{
    LocalNotificationWrapper localNotificationWrapper;
    private void Awake()
    {
        localNotificationWrapper = new LocalNotificationWrapper();
    }

    public void DoNotification(float duration)
    {
        // 通知を送信する
        localNotificationWrapper.SetNotification("Test Noti", duration + "sec noti", (int)duration);
    }

    private void OnApplicationPause(bool pause)
    {
        //一時停止(ホームボタンを押す)
        if (pause)
        {
            // DoNotification();
        }
        //再開時
        else
        {
            localNotificationWrapper.CancelAllNotification();
            localNotificationWrapper.RemoveAllDisplayedNotifications();
        }
    }
}