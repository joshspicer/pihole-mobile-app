using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using PiholeDashboard.Utils;
using PiholeDashboard.Models;
using PiholeDashboard.Functions;

namespace PiholeDashboard
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();

            MainPage = new AppShell();

            MessagingCenter.Subscribe<App>(this, "StartCommand", StartCommandAction);
            MessagingCenter.Subscribe<App>(this, "StopCommand", StopCommandAction);
        }

        private void StartCommandAction(App obj) => ModifyCommandHelper("enable");
        private void StopCommandAction(App obj) => ModifyCommandHelper("disable");

        private async void ModifyCommandHelper(string action)
        {
            // Restore values. Ensures non null.
            if (PersistenceSerializer.TryFetchConfig(out PiHoleConfig config))
            {
                await PiholeStateFunctions.ModifyHelper(action, "", false, config);
                Console.WriteLine("Completed 3D touch disable.");
                MessagingCenter.Send(Current, "RefreshDashboard");
                return;
            }

            Console.WriteLine("Tried to do 3D touch StopCommand, but failed.");
            return;
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
