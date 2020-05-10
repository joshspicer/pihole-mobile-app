using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PiholeDashboard.Models;
using System.Collections;

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
        }

        async void OpenHelp_Clicked(object sneder, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new HelpModal()));
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (!config.Uri.ToLower().Contains("http"))
            {
                var txt = "Please specify a protocol (either HTTP or HTTPS) in your URI!";
                await DisplayAlert($"Warning", txt, "Ok");
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