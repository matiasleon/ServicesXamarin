using System;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Widget;
using BroadcastReceivers.Droid.Recievers;

namespace BroadcastReceivers.Droid
{
    [Service]
    public class MyBackgroundTaskService : Service
    {
        private MyBootReceiver _myBootReceiver { get; set; }

        private const int SERVICE_RUNNING_NOTIFICATION_ID = 109900;

        private const string CHANNEL_ID = "mychannelId";

        private JobScheduler jobScheduler { get; set; }

        private const int JOB_ID = 1000;

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

        private void SetAlarm()
        {
            // intent q va a ejcutar ante un determinado tiempo
            var intent = new Intent(this, typeof(MyBootReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(this, 10, intent, PendingIntentFlags.Immutable);


            var alarmManager = GetSystemService(Context.AlarmService) as AlarmManager;
            var twoMinutes = 120000;

            alarmManager.Set(AlarmType.RtcWakeup, twoMinutes, pendingIntent);

            // que pasa si elimina de la barra de tareas la app... se tiene q inicializar un un servicio de 1er plano.
            // se ejecute una sla vez ante una determinada hora.
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