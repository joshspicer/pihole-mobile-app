using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace PiholeDashboard
{
    public partial class App : Application
    {

        public App()
        {

            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // For release versions, option to run AppCenter prebuild script to inject secrets.
            string appcenterID = AppConstant.appcenterID;
            if (appcenterID != null && appcenterID != "")
                AppCenter.Start($"ios={appcenterID}", typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
