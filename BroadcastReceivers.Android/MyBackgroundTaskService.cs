using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using BroadcastReceivers.Droid.Recievers;

namespace BroadcastReceivers.Droid
{
    [Service]
    public class MyBackgroundTaskService : Service
    {
        private MyBootReceiver _myBootReceiver { get; set; }

        private const int SERVICE_RUNNING_NOTIFICATION_ID = 109900;

        private const string CHANNEL_ID = "mychannelId";

        public MyBackgroundTaskService()
        {
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            var receiver = new MyBootReceiver();
            _myBootReceiver = receiver;

            Notification notification = CreateNotification();
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);

            // Registros servicios
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.ACTION_BOOT_COMPLETED"));
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.QUICKBOOT_POWERON"));
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.SCREEN_ON"));

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            UnregisterReceiver(_myBootReceiver);
            base.OnDestroy();
        }

        private Notification CreateNotification()
        {
            NotificationChannel notificationChannel = new NotificationChannel(CHANNEL_ID, "myChannel", NotificationImportance.High);

            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.CreateNotificationChannel(notificationChannel);

            // Instantiate the builder and set notification elements:
            Notification notification = new NotificationCompat.Builder(this, CHANNEL_ID)
                .SetChannelId(CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.abc_btn_radio_material)
                .Build();
            notification.Flags = NotificationFlags.OngoingEvent;


            // Publish the notification:
            // poner en constante para tener varias notifications
            const int notificationId = 100;
            notificationManager.Notify(notificationId, notification);

            return notification;
        }
    }
}