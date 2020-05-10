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
                                    await DisplayAlert("Success", $"Pi-Hole disabled for {duration} seconds", "nice!");
                                else
                                    await DisplayAlert("Success", $"Pi-Hole disabled.", "nice!");
                                return;
                            case "enable":
                                await DisplayAlert("Success", $"Pi-Hole re-enabled.", "nice!");
                                return;
                            default:
                                await DisplayAlert("Error", $"Unspecified Error", "ok :(");
                                return;
                        }
                    }
                }

                // If we get here, there's an error (maybe incorrect API key)
                string errStr = "Error disabling Pihole.  Ensure your complete URI and WEBPASSWORD token are entered correctly.";
                await DisplayAlert($"Error ({res.StatusCode})", errStr, "ok :(");
                Console.WriteLine($"Error with uri:{uri} ERR: {errStr}");
            }
            catch (Exception err)
            {
                string errStr = "Could not connect to PiHole service. Ensure your complete URI and WEBPASSWORD token are entered.";
                await DisplayAlert("Error!", errStr, "ok :(");
                Console.WriteLine($"{errStr}: {err}");
            }
        }
    }
}