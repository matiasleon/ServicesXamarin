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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            AppCenter.Start("4ce296f3-3a4d-4616-8d7a-2e0e9e7b9036",
                   typeof(Analytics), typeof(Crashes));

            StartServices(this);

            LoadApplication(new App());
        }

        private void StartServices(Context context)
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

            new AlarmUtil().SetAlarm(Application.Context);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}