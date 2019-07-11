
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BroadcastReceivers.Droid.Recievers
{
    [BroadcastReceiver(Enabled = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (!UserState.AnswersValid)
            {
                var myIntent = new Intent(context, typeof(MainActivity));
                myIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(myIntent);
            }
        }
    }
}
