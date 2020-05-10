using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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

        async Task ErrorAlert(string customMsg)
        {
            var wantsHelp = await DisplayAlert("Error", customMsg, "Open Help", "OK");
            if (wantsHelp)
            {
                await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
            }
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
                    var topSourcesJson = o["top_sources"];
                    var topSourcesAsDict = topSourcesJson.ToObject(typeof(Dictionary<string, int>)) as Dictionary<string, int>;

                    topSources = topSourcesAsDict;

                    // Property Changed!
                    OnPropertyChanged(nameof(topSources));
                }
                else
                {
                    string errStr = "Error fetching Pihole Summary (err=5)";
                    await ErrorAlert(errStr);
                    Console.WriteLine($"{errStr}");
                }
            }
            catch (Exception err)
            {
                string errStr = "Could not fetch connected devices. Ensure your URI/WEBPASSWORD are complete and correct. (err=6)";
                await ErrorAlert(errStr);
                Console.WriteLine($"{errStr}: {err}");
                
            }
        }
    }
}
