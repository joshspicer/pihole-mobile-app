using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PiholeDashboard.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        async void Github_Clicked(object sender, EventArgs e)
        {
            var url = "https://joshspicer.com/pihole";
            await Launcher.OpenAsync(url);
        }

        async Task ErrorAlert(string customMsg)
        {
            var wantsHelp = await DisplayAlert("Error", customMsg, "Open Help", "OK");
            if (wantsHelp)
            {
                await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
            }
        }

        async Task SuccessAlert(string customMsg)
        {
            await DisplayAlert("Success", customMsg, "Nice!");
        }

        async void Help_Clicked(object sneder, EventArgs e)
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
                var dest = App.Current.Properties["Uri"];
                var auth = App.Current.Properties["ApiKey"];
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
                        switch (operation)
                        {
                            case "disable":
                                if (duration != null && duration != "")
                                    await SuccessAlert($"Pi-Hole disabled for {duration} seconds");
                                else
                                    await SuccessAlert($"Pi-Hole disabled.");
                                return;
                            case "enable":
                                await SuccessAlert($"Pi-Hole re-enabled.");
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
    }
}