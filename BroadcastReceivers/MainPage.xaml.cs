using System.ComponentModel;
using Xamarin.Forms;

namespace BroadcastReceivers
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var userValue = e.Value;
            UserState.AnswersValid = userValue;
        }
    }
}
