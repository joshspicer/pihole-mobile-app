using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PiholeDashboard.Models;
using PiholeDashboard.Utils;
using Xamarin.Forms;

namespace PiholeDashboard.Views
{
    public partial class ConnectedClients : ContentPage
    {

        // Maps <IP address, # of requests>
        // Pi-hole returns only clients active in past 24 hrs.
        public Dictionary<string, int> topSources { get; set; }
        public PiHoleConfig config;

        bool isBackupSelected = false;


        public ConnectedClients()
        {
            InitializeComponent();
            BindingContext = this;
        }

        async protected override void OnAppearing()
        {
            Console.WriteLine("Connected Client APPEARING");

            // Restore values.  Ensures config != null.
            if (!PersistenceSerializer.TryFetchConfig(out config))
                config = new PiHoleConfig();

            // Hide Radio Buttons if there is no backup server set
            if (config.BackupUri == "")
                radioButtons.IsVisible = false;
            else
                radioButtons.IsVisible = true;

            // Auto Refresh if we have a primary URL
            if ((isBackupSelected && config.BackupApiKey != "") || config.PrimaryApiKey != "")
                await DoRefresh(showError: false);
        }

        async Task ErrorAlert(string customMsg)
        {
            var wantsHelp = await DisplayAlert("Error", customMsg, "Open Help", "OK");
            if (wantsHelp)
                await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
        }

        async void RefreshData_Clicked(object sender, EventArgs e) => await DoRefresh();

        async Task DoRefresh(bool showError = true)
        {
            try
            {
                var baseUri = isBackupSelected ? config.BackupUri : config.PrimaryUri;
                var modeApiKey = isBackupSelected ? config.BackupApiKey : config.PrimaryApiKey;
                var uri = $"{baseUri}/admin/api.php?getQuerySources&auth={modeApiKey}";

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
                    if (showError)
                    {
                        string errStr = "Error fetching Pihole Summary (err=5)";
                        await ErrorAlert(errStr);
                        Console.WriteLine($"{errStr}");
                    }
                }
            }
            catch (Exception err)
            {
                if (showError)
                {
                    string errStr = "Could not fetch connected devices. Ensure your URI/WEBPASSWORD are complete and correct. (err=6)";
                    await ErrorAlert(errStr);
                    Console.WriteLine($"{errStr}: {err}");
                }

            }
        }

        async void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            isBackupSelected = e.Value;
            await DoRefresh(showError: false);
        }
    }
}
