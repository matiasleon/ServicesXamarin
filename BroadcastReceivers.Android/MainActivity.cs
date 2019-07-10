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

namespace BroadcastReceivers.Droid
{
    [Activity(Label = "BroadcastReceivers", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            AppCenter.Start("4ce296f3-3a4d-4616-8d7a-2e0e9e7b9036",
                   typeof(Analytics), typeof(Crashes));

            // Al ser na tablet de la app no se puede eliminar del listado de tareas, con
            // registrar los eventos alcanza. 
            var receiver = new MyBootReceiver();
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.ACTION_BOOT_COMPLETED"));
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.QUICKBOOT_POWERON"));
            RegisterReceiver(receiver, new IntentFilter("android.intent.action.SCREEN_ON"));

            SetAlarm();

            LoadApplication(new App());
        }
       
        /// <summary>
        /// Inicializa servicio en 1er plano
        /// </summary>
        private void StartForegroundService()
        {
            var service = new Intent(this, typeof(MyBackgroundTaskService));
            service.AddFlags(ActivityFlags.NewTask);

            // dependiendo la version de android, se modifica la forma de inicializacion
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                StartForegroundService(service);
            }
            else
            {
                StartService(service);
            }
        }

        /// <summary>
        /// Setea alarma 
        /// </summary>
        private void SetAlarm( )
        {
            // intent q se va a ejecutar ante un determinado tiempo
            var intent = new Intent(this, typeof(MyBootReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(this, 10, intent, PendingIntentFlags.Immutable);

            var alarmManager = GetSystemService(Context.AlarmService) as AlarmManager;

            var time = DateTime.Now;
            Java.Util.Calendar calendar = Java.Util.Calendar.Instance;
            calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
            calendar.Set(time.Year, time.Month, time.Day, time.Hour, time.Minute + 4, time.Second);

            alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, AlarmManager.IntervalDay, pendingIntent);

            // que pasa si elimina de la barra de tareas la app... se tiene q inicializar un un servicio de 1er plano.
            // se ejecute una sla vez ante una determinada hora.
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}