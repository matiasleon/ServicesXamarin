using System;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Widget;
using BroadcastReceivers.Droid.Recievers;
using BroadcastReceivers.Droid.Utils;

namespace BroadcastReceivers.Droid
{
    [Service]
    public class MyBackgroundTaskService : Service
    {
        private MyBootReceiver _myBootReceiver { get; set; }

        private const int SERVICE_RUNNING_NOTIFICATION_ID = 2345;


        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            var receiver = new MyBootReceiver();
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.ACTION_BOOT_COMPLETED"));
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.QUICKBOOT_POWERON"));
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.SCREEN_ON"));
            RegisterReceiver(receiver, new IntentFilter(Intent.ActionLockedBootCompleted));
            RegisterReceiver(receiver, new IntentFilter("com.htc.intent.action.QUICKBOOT_POWERON"));
            return StartCommandResult.Sticky;
        }
    }
}