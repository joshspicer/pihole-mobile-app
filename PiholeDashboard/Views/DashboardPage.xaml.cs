using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Text.Json;
using PiholeDashboard.Models;
using System.Net.Http;
using System.Threading.Tasks;
using PiholeDashboard.Utils;

namespace PiholeDashboard.Views
{
    [DesignTimeVisible(false)]
    public partial class DashboardPage : ContentPage
    {
        public PiHoleConfig config;
        public Summary summary { get; private set; } = new Summary();
        public string lastUpdated { get; set; } = "N/A";
        public string UriBinding { get; set; } = "";
        bool isBackupSelected = false; 

        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = this;

            OnPropertyChanged(nameof(summary));
            OnPropertyChanged(nameof(lastUpdated));

            // Refresh Button
            var refresh = new TapGestureRecognizer();
            refresh.Tapped += async (s, e) => await DoRefresh();
            RefreshLabel.GestureRecognizers.Add(refresh);
        }

        async Task ErrorAlert(string customMsg)
        {
            var wantsHelp = await DisplayAlert("Error", customMsg, "Open Help", "OK");
            if (wantsHelp)
            {
                await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///configuration");

        async Task DoRefresh(bool showError=true)
        {
            try
            {
                var baseUri = isBackupSelected ? config.BackupUri : config.PrimaryUri;
                var uri = $"{baseUri}/admin/api.php?summaryRaw";

                HttpClient _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(5);
                var res = await _client.GetAsync(uri);

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();
                    summary = JsonSerializer.Deserialize<Summary>(content);
                    // Property Changed!
                    lastUpdated = DateTime.Now.ToString("hh:mm:ss tt");

                    OnPropertyChanged(nameof(summary));
                    OnPropertyChanged(nameof(lastUpdated));
                }
                else
                {
                    if (showError)
                    {
                        string errStr = "Error fetching Pihole Summary (err=3)";
                        await ErrorAlert(errStr);
                        Console.WriteLine($"{errStr}");
                    }
                }
            }
            catch (Exception err)
            {
                if (showError)
                {
                    string errStr = "Could not connect to PiHole service. Ensure your URI is set correctly. (err=4)";
                    await ErrorAlert(errStr);
                    Console.WriteLine($"{errStr}: {err}");
                }
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("DASHBOARD APPEARING!");

            // Restore values. Ensures config != null
            if (!PersistenceSerializer.TryFetchConfig(out config))
                config = new PiHoleConfig();

            DisplayRightCachedValue();

            // Refresh
            OnPropertyChanged(nameof(config));

            // Hide Radio Buttons if there is no backup server set
            if (config.BackupUri == "")
                radioButtons.IsVisible = false;
            else
                radioButtons.IsVisible = true;

            if (config != null && config.PrimaryUri != "")
                await DoRefresh(showError: false);
        }

        async void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Set class var to indicate if we are configuring primary or backup pihole.
            isBackupSelected = e.Value;
            DisplayRightCachedValue();
            await DoRefresh(showError: false);
        }

        void DisplayRightCachedValue()
        {
            // Display the right cached value
            if (isBackupSelected)
            {
                UriLabel.Text = config.BackupUri;
                int len = config.BackupApiKey.Length;
            }
            else
            {
                UriLabel.Text = config.PrimaryUri;
                int len = config.BackupApiKey.Length;
            }
        }
    }
}