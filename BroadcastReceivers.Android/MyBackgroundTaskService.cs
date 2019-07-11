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

        private JobScheduler jobScheduler { get; set; }

        private const int SERVICE_RUNNING_NOTIFICATION_ID = 2345;

        private const int JOB_ID = 1000;

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

            var nofiticationFactory = new NotificationFactory();
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, nofiticationFactory.CreateNotification(this));
            return StartCommandResult.Sticky;
        }

        private void RegisterJob()
        {
            // Se crea jobInfo, metada que usa el job scheduler para ejecutar el servicio
            var javaClass = Java.Lang.Class.FromType(typeof(Scheduler));
            var jobInfoBuilder = new JobInfo.Builder(JOB_ID, new ComponentName(this, javaClass));
            var jobInfo = jobInfoBuilder
                .SetRequiresBatteryNotLow(false)
                .SetPeriodic(900000)
                .Build();

            jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
            var scheduleResult = jobScheduler.Schedule(jobInfo);

            if (JobScheduler.ResultSuccess == scheduleResult)
            {
                Toast.MakeText(this, "Job inicializado con exito", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "Error al inicializar job", ToastLength.Long).Show();
            }
        }
    }
}