using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using PiholeDashboard.Models;
using PiholeDashboard.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

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

            // Restore values
            if (!PersistenceSerializer.TryFetchConfig(out config))
                config = new PiHoleConfig();
        }

        async Task ErrorAlert(string customMsg)
        {
            var wantsHelp = await DisplayAlert("Error", customMsg, "Open Help", "OK");
            if (wantsHelp)
                await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
        }

        async Task SuccessAlert(string customMsg)
        {
            await DisplayAlert("Success", customMsg, "Nice!");
        }

        async void Help_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
        }

        async void Disable10_Clicked(object sender, EventArgs e) => await ModifyHelper("disable", "10");
        async void Disable60_Clicked(object sender, EventArgs e) => await ModifyHelper("disable", "60");
        async void Disable300_Clicked(object sender, EventArgs e) => await ModifyHelper("disable", "300");
        async void Enable_Clicked(object sender, EventArgs e) => await ModifyHelper("enable", "");
        async void Disable_Clicked(object sender, EventArgs e) => await ModifyHelper("disable", "");


        // Generic disable Pihole Helper.
        async Task ModifyHelper(string operation, string duration)
        {
            try
            {
                var dest = isBackupSelected ? config.BackupUri : config.PrimaryUri;
                var auth = isBackupSelected ? config.BackupApiKey : config.PrimaryApiKey;
                var maybeDuration = duration != "" ? $"={duration}" : "";
                var uri = $"{dest}/admin/api.php?{operation}{maybeDuration}&auth={auth}";

                HttpClient _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(5);
                var res = await _client.GetAsync(uri);

                // Even on auth failures, return code is 200...
                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();

                    // If return contains "status", we know we were successful.
                    if (content.Contains("status"))
                    {
                        var mode = isBackupSelected ? "Backup" : "Primary";

                        switch (operation)
                        {
                            case "disable":
                                if (duration != null && duration != "")
                                    await SuccessAlert($"{mode} pi-hole disabled for {duration} seconds");
                                else
                                    await SuccessAlert($"{mode} pi-hole disabled.");
                                return;
                            case "enable":
                                await SuccessAlert($"{mode} pi-hole re-enabled.");
                                return;
                            default:
                                await ErrorAlert("Unspecified Error (err=0)");
                                return;
                        }
                    }
                }

                // If we get here, there's an error (maybe incorrect API key)
                string errStr = "Error toggling Pi-hole. Please check your WEBPASSWORD. (err=1)";
                await ErrorAlert(errStr);
                Console.WriteLine($"Error with uri:{uri} ERR: {errStr}");
            }
            catch (Exception err)
            {
                string errStr = "Could not connect to Pi-Hole service (err=2)";
                await ErrorAlert(errStr);
                Console.WriteLine($"{errStr}: {err}");
            }
        }

        void RadioButton_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e) => isBackupSelected = e.Value;
    }
}