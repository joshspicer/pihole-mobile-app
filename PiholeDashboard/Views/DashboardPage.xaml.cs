using System;
using System.ComponentModel;
using Xamarin.Forms;
using Newtonsoft.Json;
using PiholeDashboard.Models;
using System.Net.Http;

namespace PiholeDashboard.Views
{
    [DesignTimeVisible(false)]
    public partial class DashboardPage : ContentPage
    {
        public PiHoleConfig config { get; } = new PiHoleConfig();
        public Summary summary { get; private set; } = new Summary();

        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = this;

            OnPropertyChanged(nameof(summary));
            OnPropertyChanged(nameof(config));
        }


        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        async void RefreshData_Clicked(object sender, EventArgs e)
        {
            try
            {
                //var uri = $"{App.Current.Properties["Uri"]}/admin/api.php?summary";
                var uri = $"{App.Current.Properties["Uri"]}/admin/api.php?summaryRaw&getQuerySources&topClientsBlocked&auth";
                uri += App.Current.Properties.ContainsKey("ApiKey") ? $"={App.Current.Properties["ApiKey"]}" : "";

                Console.WriteLine($"URI === {uri}");

                HttpClient _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(5);
                var res = await _client.GetAsync(uri);

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();
                    summary = JsonConvert.DeserializeObject<Summary>(content);
                    // Property Changed!
                    OnPropertyChanged(nameof(summary));
                }
                else
                {
                    string errStr = "Error fetching Pihole Summary";
                    await DisplayAlert($"Error ({res.StatusCode})", errStr, "ok :(");
                    Console.WriteLine($"{errStr}");
                }
            }
            catch (Exception err)
            {
                string errStr = "Could not connect to PiHole service. Ensure your complete URI is displayed below (including the protocol HTTP or HTTPS).";
                await DisplayAlert("Error!", errStr, "ok :(");
                Console.WriteLine($"{errStr}: {err}");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set 
            if (App.Current.Properties.ContainsKey("Uri"))
            {
                var url = App.Current.Properties["Uri"] as string;
                Console.WriteLine(url);
                config.Uri = url;
            }

            // Set ApiKey
            if (App.Current.Properties.ContainsKey("ApiKey"))
            {
                var key = App.Current.Properties["ApiKey"] as string;
                Console.WriteLine(key);
                config.ApiKey = key;
            }

            // Refresh
            OnPropertyChanged(nameof(config));
        }
    }
}