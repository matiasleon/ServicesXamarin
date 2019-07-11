using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Microsoft.AppCenter.Analytics;

namespace BroadcastReceivers.Droid.Recievers
{
    [BroadcastReceiver(Enabled = true, Exported =true, DirectBootAware = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted,
                          Intent.ActionLockedBootCompleted,
                          "android.intent.action.SCREEN_ON",
                          "android.intent.action.QUICKBOOT_POWERON",
                          "com.htc.intent.action.QUICKBOOT_POWERON" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class MyBootReceiver : BroadcastReceiver
    {
        public MyBootReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (!UserState.AnswersValid)
            {
                var myIntent = new Intent(context, typeof(MainActivity));
                myIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(myIntent);
                RegisterEvent();
            }
        }

        private void RegisterEvent()
        {
            var date = DateTime.Now;
            var msg = string.Format("Receiver ejecutado: {0}", date.ToString());
            Analytics.TrackEvent(msg);
        }
    }
}
    