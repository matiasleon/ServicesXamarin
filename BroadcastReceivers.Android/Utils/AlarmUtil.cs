using System;
using Android.App;
using Android.Content;
using BroadcastReceivers.Droid.Recievers;
using Java.Lang;
using Java.Util;

namespace BroadcastReceivers.Droid.Utils
{
    public class AlarmUtil
    {
        public AlarmUtil()
        {
        }

        /// <summary>
        /// Setea alarma 
        /// </summary>
        public void SetAlarm(Context context)
        {
            // Intent q se va a ejecutar ante un determinado tiempo
            var intent = new Intent(context, typeof(AlarmReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(context, 10, intent, PendingIntentFlags.Immutable);

            var alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;

            //Setear alarma a a las 7 AM desde dnd estoy parado.
            var now = DateTime.Now;
            var futureDate = new DateTime(now.Year, now.Month, now.Day + 1, 14, 1,0);
            var futureDateMilis = (long)futureDate.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            //long ringTime = JavaSystem.CurrentTimeMillis() + (long)TimeSpan.FromMinutes(2).TotalMilliseconds;

            alarmManager.SetRepeating(AlarmType.RtcWakeup, futureDateMilis, AlarmManager.IntervalDay, pendingIntent);
        }
    }
}
