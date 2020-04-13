﻿using System;
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

        async void Save_Clicked(object sender, EventArgs e)
        {
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