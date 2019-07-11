using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;

namespace BroadcastReceivers.Droid.Utils
{
    public class NotificationFactory
    {
        private const string CHANNEL_ID = "mychannelId";

        public Notification CreateNotification(Context context)
        {
            NotificationManager notificationManager =
                    context.GetSystemService(Context.NotificationService) as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(CHANNEL_ID, "myChannel", NotificationImportance.High);
                notificationManager.CreateNotificationChannel(notificationChannel);
            }

                
            // Instantiate the builder and set notification elements:
            Notification notification = new NotificationCompat.Builder(context, CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.abc_btn_radio_material)
                .Build();
            
            return notification;
        }
    }
}
