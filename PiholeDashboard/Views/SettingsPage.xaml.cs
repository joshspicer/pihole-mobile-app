using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiholeDashboard.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class SettingsPage : ContentPage
    {

        HttpClient _client = new HttpClient();


        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        async void Disable10_Clicked(object sender, EventArgs e)
        {
            await DisableHelper("10");
        }

        // Generic disable Pihole Helper.
        async Task DisableHelper(string duration)
        {
            try
            {
                var dest = App.Current.Properties["Uri"];
                var auth = App.Current.Properties["ApiKey"];
                var uri = $"http://{dest}/admin/api.php?disable={duration}&auth={auth}";
                var res = await _client.GetAsync(uri);

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();
                    await DisplayAlert("Success", $"PiHole disabled for {duration} seconds", "nice!");
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