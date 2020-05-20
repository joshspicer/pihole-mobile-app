using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PiholeDashboard.Views
{
    public partial class HelpModal : ContentPage
    {
        public HelpModal()
        {
            InitializeComponent();
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Config_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));

    }
}
