using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BroadcastReceivers.Droid.Recievers;
using Android.Content;
using Android.App.Job;
using Android.Support.Design.Widget;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Java.Util;
using Java.Lang;
using BroadcastReceivers.Droid.Utils;

namespace BroadcastReceivers.Droid
{
    [Activity(Label = "BroadcastReceivers", Icon = "@mipmap/icon", MainLauncher =true, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private JobScheduler jobScheduler { get; set; }

        private const int JOB_ID = 1000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			AppCenter.Start("4ce296f3-3a4d-4616-8d7a-2e0e9e7b9036",
                   typeof(Analytics), typeof(Crashes));

            StartServices();

            LoadApplication(new App());
        }

        private void StartServices()
        {
            var receiver = new MyBootReceiver();
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.ACTION_BOOT_COMPLETED"));
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.QUICKBOOT_POWERON"));
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.SCREEN_ON"));
            RegisterReceiver(receiver, new IntentFilter(Intent.ActionLockedBootCompleted));
            RegisterReceiver(receiver, new IntentFilter("com.htc.intent.action.QUICKBOOT_POWERON"));
            new AlarmUtil().SetAlarm(Application.Context);
            RegisterJob();

        }

        private void StartForegroundService(Context context)
        {
            var intent = new Intent(context, typeof(MyBackgroundTaskService));
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
        }


        private void RegisterJob()
        {
            // Se crea jobInfo, metadata que usa el job scheduler para ejecutar el servicio
            var javaClass = Java.Lang.Class.FromType(typeof(Scheduler));
            var jobInfoBuilder = new JobInfo.Builder(JOB_ID, new ComponentName(this, javaClass));
            var jobInfo = jobInfoBuilder
                .SetRequiresBatteryNotLow(false)
                .SetRequiredNetworkType(NetworkType.Any) // wifi connection
                .SetPeriodic(900000)
                .SetPersisted(true) 
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}