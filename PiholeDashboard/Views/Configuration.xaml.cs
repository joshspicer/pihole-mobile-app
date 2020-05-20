using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PiholeDashboard.Models;
using System.Collections;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;

namespace PiholeDashboard.Views
{
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public PiHoleConfig config { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            config = new PiHoleConfig();
            BindingContext = this;

            // Restore values
            if (App.Current.Properties.ContainsKey("Uri"))
                Uri.Text = App.Current.Properties["Uri"].ToString();
            if (App.Current.Properties.ContainsKey("ApiKey"))
                ApiKey.Text = App.Current.Properties["ApiKey"].ToString();

        }

        async Task ErrorAlert(string customMsg)
        {
            var wantsHelp = await DisplayAlert("Error", customMsg, "Open Help", "OK");
            if (wantsHelp)
            {
                await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
            }
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
                    config.ApiKey = result.Text;
                    ApiKey.Text = config.ApiKey;
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
            if (!config.Uri.ToLower().Contains("http"))
            {
                var txt = "Please specify a protocol (HTTP/HTTPS) in your URI!";
                await ErrorAlert(txt);
                return;
            }

            var keys = new List<string>() { "ApiKey", "Uri" };
            foreach (var k in keys)
            {
                // Save 
                if (App.Current.Properties.ContainsKey(k))
                    App.Current.Properties[k] = k == "ApiKey" ? config.ApiKey : config.Uri;
                else
                    App.Current.Properties.Add(k, k == "ApiKey" ? config.ApiKey : config.Uri);
            }

            // Pop this model.
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}