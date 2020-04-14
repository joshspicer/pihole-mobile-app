using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        async void Disable10_Clicked(object sender, EventArgs e) => await DisableHelper("10");
        async void Disable60_Clicked(object sender, EventArgs e) => await DisableHelper("60");
        async void Disable300_Clicked(object sender, EventArgs e) => await DisableHelper("300");

        // Generic disable Pihole Helper.
        async Task DisableHelper(string duration)
        {
            try
            {
                var dest = App.Current.Properties["Uri"];
                var auth = App.Current.Properties["ApiKey"];
                var uri = $"http://{dest}/admin/api.php?disable={duration}&auth={auth}";

                HttpClient _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(5);
                var res = await _client.GetAsync(uri);

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();
                    if (!content.Contains("Not"))
                    {
                        await DisplayAlert("Success", $"PiHole disabled for {duration} seconds", "nice!");
                    }
                }
                else
                {
                    string errStr = "Error disabling Pihole";
                    await DisplayAlert($"Error ({res.StatusCode})", errStr, "ok :(");
                    Console.WriteLine($"{errStr}");
                }
            }
            catch (Exception err)
            {
                string errStr = "Could not connect to PiHole service";
                await DisplayAlert("Error!", errStr, "ok :(");
                Console.WriteLine($"{errStr}: {err}");
            }
        }
    }
}