using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using PiholeDashboard.Models;
using PiholeDashboard.Views;
using PiholeDashboard.ViewModels;
using System.Net.Http;
using System.Globalization;

namespace PiholeDashboard.Views
{
    [DesignTimeVisible(false)]
    public partial class DashboardPage : ContentPage
    {
        public PiHoleConfig config { get; } = new PiHoleConfig();
        public Summary summary { get; private set; } = new Summary();

        HttpClient _client = new HttpClient();

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
                var uri = $"http://{App.Current.Properties["Uri"]}/admin/api.php?summary";
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
                string errStr = "Could not connect to PiHole service";
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