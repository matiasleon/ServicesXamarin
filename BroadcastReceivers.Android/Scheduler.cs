using System;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Widget;
using Microsoft.AppCenter.Analytics;

namespace BroadcastReceivers.Droid
{
	[Service(Name = "BroadcastReceivers.Droid.Scheduler",
			 Permission = "android.permission.BIND_JOB_SERVICE")]
	public class Scheduler : JobService
	{
		public Scheduler()
		{
		}

		public override bool OnStartJob(JobParameters @params)
		{
            Task.Run(async () => {

                
                await Task.Delay(5000);
                var date = DateTime.Now;
                var msg = string.Format("Schedueler Job ejecutado: {0}", date.ToString());
                Toast.MakeText(Application.Context, msg, ToastLength.Long).Show();
                JobFinished(@params, false);

            });

			return true;
		}

		public override bool OnStopJob(JobParameters @params)
		{
			return true;
		}
	}
}
    