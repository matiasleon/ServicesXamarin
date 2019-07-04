using System;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Widget;

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
			var myIntent = new Intent(this.BaseContext, typeof(MainActivity));
			myIntent.AddFlags(ActivityFlags.NewTask);
			this.BaseContext.StartActivity(myIntent);
			Toast.MakeText(this.BaseContext, "scheduler / boot / on screen on", ToastLength.Long).Show();
			JobFinished(@params, false);

			return false;
		}

		public override bool OnStopJob(JobParameters @params)
		{
			return true;
		}
	}
}
