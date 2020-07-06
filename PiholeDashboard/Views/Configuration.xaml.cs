using System;
using System.ComponentModel;
using Xamarin.Forms;
using PiholeDashboard.Models;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;
using Xamarin.Essentials;
using PiholeDashboard.Utils;
namespace PiholeDashboard.Views
{
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public PiHoleConfig config;

        public string UriBinding { get; set; }
        public string ApiKeyBinding { get; set; }

        bool isBackupSelected = false;

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = this;

            // Restore values
            if (!PersistenceSerializer.TryFetchConfig(out config))
                config = new PiHoleConfig();

            UriLabel.Text = config.PrimaryUri;
            ApiKeyLabel.Text = config.PrimaryApiKey;

            // Refresh Button
            var refresh = new TapGestureRecognizer();
            refresh.Tapped += async (s, e) => await Github_Clicked();
            github.GestureRecognizers.Add(refresh);
        }

        async Task Github_Clicked()
        {
            var url = "https://joshspicer.com/pihole";
            await Launcher.OpenAsync(url);
        }

        async Task ErrorAlert(string customMsg)
        {
            var wantsHelp = await DisplayAlert("Error", customMsg, "Open Help", "OK");
            if (wantsHelp)
                await Shell.Current.GoToAsync("///help");
        }

        async void QR_Clicked(object sender, EventArgs e)
        {
            var scanPage = new ZXingScannerPage();

            scanPage.OnScanResult += (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    if (isBackupSelected)
                        config.PrimaryApiKey = result.Text;
                    else
                        config.BackupApiKey = result.Text;

                    ApiKeyLabel.Text = result.Text;
                });
            };

            // Navigate to our scanner page
            await Navigation.PushAsync(scanPage);
        }

        async void OpenHelp_Clicked(object sneder, EventArgs e)
        {
            await Shell.Current.GoToAsync("///help");
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (!UriBinding.ToLower().Contains("http"))
            {
                var txt = "Please specify a protocol (HTTP/HTTPS) in your URI!";
                await ErrorAlert(txt);
                return;
            }

            DisplayValues();

            // Save
            PersistenceSerializer.SerializeAndSaveConfig(config);

            // Pop this model.
            await Shell.Current.GoToAsync("///browse");
        }

        void DisplayValues()
        {
            if (isBackupSelected)
            {
                config.BackupUri = UriBinding;
                config.BackupApiKey = ApiKeyBinding;
            }
            else
            {
                config.PrimaryUri = UriBinding;
                config.PrimaryApiKey = ApiKeyBinding;
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("///browse");

        void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Write whatever is written where user is dismissing
            DisplayValues();

            // Set class var to indicate if we are configuring primary or backup pihole.
            isBackupSelected = e.Value;

            // Display the right cached value
            if (isBackupSelected)
            {
                UriLabel.Text = config.BackupUri;
                ApiKeyLabel.Text = config.BackupApiKey;
            }
            else
            {
                UriLabel.Text = config.PrimaryUri;
                ApiKeyLabel.Text = config.PrimaryApiKey;
            }
        }
    }
}