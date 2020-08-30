using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using PiholeDashboard.Models;
using PiholeDashboard.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;
using PiholeDashboard.Functions;

namespace PiholeDashboard.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingsPage : ContentPage
    {
        bool isBackupSelected = false;
        public PiHoleConfig config;

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        
        protected override void OnAppearing()
        {
            Console.WriteLine("Settings APPEARING");

            // Restore values. Ensures non null.
            if (!PersistenceSerializer.TryFetchConfig(out config))
                config = new PiHoleConfig();

            // Hide Radio Buttons if there is no backup server set
            if (config.BackupUri == "")
                radioButtons.IsVisible = false;
            else
                radioButtons.IsVisible = true;
        }

        async Task ErrorAlert(string customMsg)
        {
            var wantsHelp = await DisplayAlert("Error", customMsg, "Open Help", "OK");
            if (wantsHelp)
                await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
        }

        async Task SuccessAlert(string customMsg)
        {
            await DisplayAlert("Success", customMsg, "nice!");
        }

        async void Help_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
        }

        async void RespondToModify(bool status, string successMsg = "")
        {
            if (status)
                await SuccessAlert(successMsg);
            else
                await ErrorAlert("Please check your configuration settings!");
        }

        async void Disable10_Clicked(object sender, EventArgs e) =>     RespondToModify(await PiholeStateFunctions.ModifyHelper("disable", "10", isBackupSelected, config),  "Disabled for 10 seconds");
        async void Disable60_Clicked(object sender, EventArgs e) =>     RespondToModify(await PiholeStateFunctions.ModifyHelper("disable", "60", isBackupSelected, config),  "Disabled for 60 seconds");
        async void Disable300_Clicked(object sender, EventArgs e) =>    RespondToModify(await PiholeStateFunctions.ModifyHelper("disable", "300", isBackupSelected, config), "Disabled for 5 minutes");
        async void Enable_Clicked(object sender, EventArgs e) =>        RespondToModify(await PiholeStateFunctions.ModifyHelper("enable", "", isBackupSelected, config),     "Filtering enabled.");
        async void Disable_Clicked(object sender, EventArgs e) =>       RespondToModify(await PiholeStateFunctions.ModifyHelper("disable", "", isBackupSelected, config),    "Filter disabled.");

        void RadioButton_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e) => isBackupSelected = e.Value;
    }
}