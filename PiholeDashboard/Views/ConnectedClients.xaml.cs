using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace PiholeDashboard.Views
{
    public partial class ConnectedClients : ContentPage
    {

        // Maps <IP address, # of requests>
        // Pi-hole returns only clients active in past 24 hrs.
        public Dictionary<string, int> topSources { get; set; }

        public ConnectedClients()
        {
            InitializeComponent();
            BindingContext = this;
        }

        async void RefreshData_Clicked(object sender, EventArgs e)
        {
            try
            {
                var uri = $"{App.Current.Properties["Uri"]}/admin/api.php?getQuerySources&auth={App.Current.Properties["ApiKey"]}";

                HttpClient _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(5);
                var res = await _client.GetAsync(uri);

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();

                    JObject o = JObject.Parse(content);
                    var aa = o["top_sources"];
                    var bb = aa.ToObject(typeof(Dictionary<string, int>)) as Dictionary<string, int>;

                    topSources = bb;

                    // Property Changed!
                    OnPropertyChanged(nameof(topSources));
                }
                else
                {
                    string errStr = "Error fetching Pihole Summary";
                    await DisplayAlert($"Error", errStr, "ok :(");
                    Console.WriteLine($"{errStr}");
                }
            }
            catch (Exception err)
            {
                string errStr = "Could not fetch Connected Devices. Ensure your complete URI is displayed below (including the protocol HTTP or HTTPS).";
                await DisplayAlert("Error!", errStr, "ok :(");
                Console.WriteLine($"{errStr}: {err}");
                
            }
        }
    }
}
