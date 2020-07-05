using System;
using System.ComponentModel;
using Xamarin.Forms;
using PiholeDashboard.Models;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;

namespace PiholeDashboard.Views
{
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public PiHoleConfig config { get; set; }

        public string UriBinding { get; set; }
        public string ApiKeyBinding { get; set; }

        bool isBackupSelected = false;

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = this;

            // Restore values
            if (App.Current.Properties.ContainsKey("config"))
                config = (PiHoleConfig)App.Current.Properties["config"];
            else
                config = new PiHoleConfig();

            UriLabel.Text = config.PrimaryUri;
            ApiKeyLabel.Text = config.PrimaryApiKey;
        }

        async Task ErrorAlert(string customMsg)
        {
            var wantsHelp = await DisplayAlert("Error", customMsg, "Open Help", "OK");
            if (wantsHelp)
                await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
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
            await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (!UriBinding.ToLower().Contains("http"))
            {
                var txt = "Please specify a protocol (HTTP/HTTPS) in your URI!";
                await ErrorAlert(txt);
                return;
            }

            WriteValues();

            // Save 
            if (App.Current.Properties.ContainsKey("config"))
                App.Current.Properties["config"] = config;
            else
                App.Current.Properties.Add("config", config);

            // Pop this model.
            await Navigation.PopModalAsync();
        }

        void WriteValues() {
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

        async void Cancel_Clicked(object sender, EventArgs e) => await Navigation.PopModalAsync();

        void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Write whatever is written where user is dismissing
            WriteValues();

            // Set class var to indicate if we are configuring primary or backup pihole.
            isBackupSelected = e.Value;

            // Display the right cached value
            if (isBackupSelected)
            {
                UriLabel.Text = config.BackupUri;
                ApiKeyLabel.Text = config.BackupApiKey;
            } else
            {
                UriLabel.Text = config.PrimaryUri;
                ApiKeyLabel.Text = config.PrimaryApiKey;
            }

        }
    }
}