using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Microsoft.AppCenter.Analytics;

namespace BroadcastReceivers.Droid.Recievers
{
    [BroadcastReceiver(Enabled = true, Exported =true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted,
                          "android.intent.action.SCREEN_ON"})]
    public class MyBootReceiver : BroadcastReceiver
    {
        public MyBootReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var myIntent = new Intent(context, typeof(MainActivity));
            myIntent.AddFlags(ActivityFlags.NewTask);
            context.StartActivity(myIntent);
            var date = DateTime.Now;
            var msg = string.Format("Receiver ejecutado: {0}", date.ToString());
            Analytics.TrackEvent(msg);
        }
    }
}
    